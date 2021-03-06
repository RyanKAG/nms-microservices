using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;
using DeviceManagement.API.Repository;
using DeviceManagement.API.Utils;
using MassTransit;
using Newtonsoft.Json;

namespace DeviceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseControllerAsync<TModel, TReadDto, TUpdateDto, TCreateDto, CEvent, DEvent, UEvent> : ControllerBase
        where TModel : BaseModel
        where TReadDto : class, IDto
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IRepository<TModel> Repository;
        protected readonly IMapper Mapper;
        protected readonly ILogger<TModel> Logger;
        protected readonly IPublishEndpoint Pub;


        public BaseControllerAsync(IRepository<TModel> repository, IMapper mapper, ILogger<TModel> logger, IPublishEndpoint pub)
        {
            Repository = repository;
            Mapper = mapper;
            Logger = logger;
            Pub = pub;
        }

        [HttpHead("{id}")]
        public virtual async Task<IActionResult> Check(Guid id)
        {
            Logger.LogInformation("Checking for {model} existance with ID: {id}", nameof(TModel), id);
            var entity = await Repository.GetByIdAsync(id);

            return entity != null ? Ok() : NotFound();
        }
        
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAllAsync(CancellationToken cancellationToken, [FromQuery] PaginationDto paginationDto)
        {
            Logger.LogInformation(LogEvents.ListResourses, "Retrieving All resources");
            var pagedList = await Repository.GetAllAsync(cancellationToken, paginationDto);
            await Pub.Publish(Mapper.Map<DEvent>(new Device(){Id = Guid.NewGuid()}));

            var paginationMeta = new
            {
                pagedList.TotalCount,
                pagedList.PageSize,
                pagedList.CurrentPage,
                pagedList.TotalPages,
                pagedList.HasNext,
                pagedList.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMeta));
            return Ok(Mapper.Map<IEnumerable<DeviceReadDto>>(pagedList));
        }
        // GET api/Model/{id} One use with id
        //[Authorize]
        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public virtual async Task<ActionResult<TReadDto>> GetAsync(Guid id)
        {
            Logger.LogInformation(LogEvents.GetResourse, "Retrieving a resource");

            var entity = await Repository.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                    nameof(TModel), id.ToString());
                return NotFound();
            }

            return Ok(Mapper.Map<TReadDto>(entity));
        }
        
        [HttpPost("range")]
        [ActionName(nameof(GetListAsync))]
        public virtual async Task<ActionResult<TReadDto>> GetListAsync(IEnumerable<Guid> ids)
        {
            Logger.LogInformation(LogEvents.GetResourse, "Retrieving a resource");

            var entities = await Repository.GetByListOfIdsAsync(ids);

            if (entities == null)
            {
                // _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                //     nameof(TModel), id.ToString());
                return NotFound();
            }

            return Ok(Mapper.Map<IEnumerable<TReadDto>>(entities));
        }

        // POST api/Model
        [HttpPost]
        public virtual async Task<ActionResult<TReadDto>> CreateAsync(TCreateDto createDto)
        {
            var entity = Mapper.Map<TModel>(createDto);
            await Repository.CreateAsync(entity);
            await Repository.SaveChangesAsync();
            
            await Pub.Publish(Mapper.Map<CEvent>(entity));
            
            return CreatedAtAction(nameof(GetAsync), new {id = entity.Id}, new { });
        }

        // PUT api/Model/{id}
        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Update(Guid id, TUpdateDto updateDto)
        {
            var entity = await Repository.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                    nameof(TModel), id.ToString());
                return NotFound();
            }

            Mapper.Map(updateDto, entity);

            Repository.UpdateAsync(entity);
            var isSaved = await Repository.SaveChangesAsync();

            return isSaved ? NoContent() : Problem("Could not update resource");
        }

        // PATCH api/s/{id}
        [HttpPatch("{id}")]
        public virtual async Task<ActionResult> PartialUpdate(Guid id, JsonPatchDocument<TUpdateDto> patchDoc)
        {
            Logger.LogInformation(LogEvents.UpdateResourse, "Updating (patching) resourse {id}");
            var entity = await Repository.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogWarning(LogEvents.GetResourseNotFound, "Get({id}) NOT FOUND");
                return NotFound();
            }

            // mapp TModel to user dto then try to apple the patch doc to it.
            var opToPatch = Mapper.Map<TUpdateDto>(entity);
            patchDoc.ApplyTo(opToPatch, ModelState);
            if (!TryValidateModel(entity))
            {
                return ValidationProblem(ModelState);
            }

            // Map patched user to user which will update Db then save
            Mapper.Map(opToPatch, entity);
            Repository.UpdateAsync(entity);
            await Repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/s/{id}
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
            var entity = await Repository.GetByIdAsync(id);

            if (entity == null)
                return NoContent();
            
            Repository.Delete(entity);

            try
            {
                var isSuccessful = await Repository.SaveChangesAsync();
                if (!isSuccessful)
                    return Problem(detail: "A server error has occured");
            }
            catch (Exception e)
            {
                Logger.LogCritical(exception: e, message: "Critical Error while saving changes");
            }

            
            await Pub.Publish(Mapper.Map<DEvent>(entity));
            return Ok();
        }
    }
}