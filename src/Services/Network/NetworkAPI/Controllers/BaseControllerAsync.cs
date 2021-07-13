using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MassTransit.RabbitMqTransport.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using NetworkAPI.Dtos;
using NetworkAPI.Models;
using NetworkAPI.Repository;
using NetworkAPI.Utils;
using Newtonsoft.Json;

namespace NetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseControllerAsync<TModel, TReadDto, TUpdateDto, TCreateDto, CEvent> : ControllerBase
        where TModel : BaseModel
        where TReadDto : class, IDto
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IRepository<TModel> Repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TModel> _logger;
        private readonly IPublishEndpoint _pub;

        public BaseControllerAsync(IRepository<TModel> repository, IMapper mapper, ILogger<TModel> logger, IPublishEndpoint pub)
        {
            Repository = repository;
            _mapper = mapper;
            _logger = logger;
            _pub = pub;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAllAsync(CancellationToken cancellationToken, [FromQuery] PaginationDto paginationDto)
        {
            _logger.LogInformation(LogEvents.ListResourses, "Retrieving All resources");
            var pagedList = await Repository.GetAllAsync(cancellationToken, paginationDto);
            
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
            return Ok(pagedList);
        }

        
        [HttpHead("{id}")]
        public virtual async Task<IActionResult> Check(Guid id)
        {
            _logger.LogInformation("Checking for {model} existance with ID: {id}", nameof(TModel), id);
            var entity = await Repository.GetByIdAsync(id);

            return entity != null ? Ok() : NotFound();
        }
        
        [HttpPost("range")]
        [ActionName(nameof(GetListAsync))]
        public virtual async Task<ActionResult<TReadDto>> GetListAsync(IEnumerable<Guid> ids)
        {
            _logger.LogInformation(LogEvents.GetResourse, "Retrieving a resource");

            var entities = await Repository.GetByListOfIdsAsync(ids);

            if (entities == null)
            {
                // _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                //     nameof(TModel), id.ToString());
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<TReadDto>>(entities));
        }
        
        // GET api/Model/{id} One use with id
        //[Authorize]
        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public virtual async Task<ActionResult<TReadDto>> GetAsync(Guid id)
        {
            _logger.LogInformation(LogEvents.GetResourse, "Retrieving a resource");

            var entity = await Repository.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                    nameof(TModel), id.ToString());
                return NotFound();
            }

            return Ok(_mapper.Map<TReadDto>(entity));
        }

        // POST api/Model
        [HttpPost]
        public virtual async Task<ActionResult<TReadDto>> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TModel>(createDto);
            await Repository.CreateAsync(entity);

            await Repository.SaveChangesAsync();

            await _pub.Publish(_mapper.Map<CEvent>(entity));
            
            return CreatedAtAction(nameof(GetAsync), new {id = entity.Id}, new { });
        }

        // PUT api/Model/{id}
        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Update(Guid id, TUpdateDto updateDto)
        {
            var entity = await Repository.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                    nameof(TModel), id.ToString());
                return NotFound();
            }

            _mapper.Map(updateDto, entity);

            Repository.UpdateAsync(entity);
            var isSaved = await Repository.SaveChangesAsync();

            return isSaved ? NoContent() : Problem("Could not update resource");
        }

        // PATCH api/s/{id}
        [HttpPatch("{id}")]
        public virtual async Task<ActionResult> PartialUpdate(Guid id, JsonPatchDocument<TUpdateDto> patchDoc)
        {
            _logger.LogInformation(LogEvents.UpdateResourse, "Updating (patching) resourse {id}");
            var entity = await Repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning(LogEvents.GetResourseNotFound, "Get({id}) NOT FOUND");
                return NotFound();
            }

            // mapp TModel to user dto then try to apple the patch doc to it.
            var opToPatch = _mapper.Map<TUpdateDto>(entity);
            patchDoc.ApplyTo(opToPatch, ModelState);
            if (!TryValidateModel(entity))
            {
                return ValidationProblem(ModelState);
            }

            // Map patched user to user which will update Db then save
            _mapper.Map(opToPatch, entity);
            Repository.UpdateAsync(entity);
            await Repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/s/{id}
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
            var entity = await Repository.GetByIdAsync(id);

            Repository.Delete(entity);

            try
            {
                var isSuccessful = await Repository.SaveChangesAsync();
                if (!isSuccessful)
                    return Problem(detail: "A server error has occured");
            }
            catch (Exception e)
            {
                _logger.LogCritical(exception: e, message: "Critical Error while saving changes");
            }

            return Ok();        
        }
    }
}