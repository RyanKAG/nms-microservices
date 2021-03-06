using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Auth.API.Dtos;
using Auth.API.Repository;
using Auth.API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Auth.API.Models;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseControllerAsync<TModel, TReadDto, TUpdateDto, TCreateDto> : ControllerBase
        where TModel : class, IModel
        where TReadDto : class, IDto
        where TCreateDto : class
        where TUpdateDto : class
    {
        private readonly IRepository<TModel> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TModel> _logger;


        public BaseControllerAsync(IRepository<TModel> repository, IMapper mapper, ILogger<TModel> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(LogEvents.ListResourses, "Retrieving All resources");
            var entities = await _repository.GetAllAsync(cancellationToken);


            return Ok(_mapper.Map<IEnumerable<TReadDto>>(entities));
        }

        // GET api/Model/{id} One use with id
        //[Authorize]
        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public virtual async Task<ActionResult<TReadDto>> GetAsync(Guid id)
        {
            _logger.LogInformation(LogEvents.GetResourse, "Retrieving a resource");

            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                    nameof(TModel), id.ToString());
                return NotFound();
            }

            return Ok(_mapper.Map<TReadDto>(entity));
        }
        
        [HttpPost("range")]
        [ActionName(nameof(GetListAsync))]
        public virtual async Task<ActionResult<TReadDto>> GetListAsync(IEnumerable<Guid> ids)
        {
            _logger.LogInformation(LogEvents.GetResourse, "Retrieving a resource");

            var entities = await _repository.GetByListOfIdsAsync(ids);

            if (entities == null)
            {
                // _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                //     nameof(TModel), id.ToString());
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<TReadDto>>(entities));
        }

        // POST api/Model
        [HttpPost]
        public virtual async Task<ActionResult<TReadDto>> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TModel>(createDto);
            await _repository.CreateAsync(entity);

            return CreatedAtAction(nameof(GetAsync), new {id = entity.Id}, new { });
        }

        // PUT api/Model/{id}
        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Update(Guid id, TUpdateDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist",
                    nameof(TModel), id.ToString());
                return NotFound();
            }

            _mapper.Map(updateDto, entity);

            _repository.UpdateAsync(entity);
            var isSaved = await _repository.SaveChangesAsync();

            return isSaved ? NoContent() : Problem("Could not update resource");
        }

        // PATCH api/s/{id}
        [HttpPatch("{id}")]
        public virtual async Task<ActionResult> PartialUpdate(Guid id, JsonPatchDocument<TUpdateDto> patchDoc)
        {
            _logger.LogInformation(LogEvents.UpdateResourse, "Updating (patching) resourse {id}");
            var entity = await _repository.GetByIdAsync(id);
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
            _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/s/{id}
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            _repository.Delete(entity);

            try
            {
                var isSuccessful = await _repository.SaveChangesAsync();
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