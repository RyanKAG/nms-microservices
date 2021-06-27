using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeviceAPI.Dtos;
using DeviceAPI.Models;
using DeviceAPI.Repository;
using DeviceAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
namespace DeviceAPI.Base
{
    public class BaseControllerAsync<TModel, TReadDto, TUpdateDto, TCreateDto> : ControllerBase
        where TModel : BaseModel
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

        public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAllAsync()
        {
            _logger.LogInformation(LogEvents.ListResourses, "Retrieving All resources");
            var entities = await _repository.GetAllAsync();
            
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
               _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist", nameof(TModel), id.ToString());
               return NotFound();
           }

           return Ok(_mapper.Map<TReadDto>(entity));
        }

        // POST api/Model
        [HttpPost]
        public virtual async Task<ActionResult<TReadDto>> CreateAsync(TCreateDto createDto)
        {
            
        }
        // PUT api/Model/{id}
        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Update(Guid id, TUpdateDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);
            
            if (entity == null)
            {
                _logger.LogInformation(LogEvents.GetResourseNotFound, "{modelName} of ID {id}, does not exist", nameof(TModel), id.ToString());
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
            var result = await _repository.UpdateAsync(entity);
            if (!result.Success) return BadRequest(result.Message);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/s/{id}
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(Guid id)
        {
           
        }
        
    }
}