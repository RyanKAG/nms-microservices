using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        where TModel : IModel
        where TReadDto : class
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

        public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAll()
        {
            _logger.LogInformation(LogEvents.ListResourses, "Listing resourses ");
            var opItems = await _repository.GetAllAsync();
            if (opItems.Count() == 0) _logger.LogInformation(LogEvents.ListResourses, "No resourses");
            // make the repo get all usertypes to map it with Model 
            //Return a Maped the op to opDTO 
            return Ok(_mapper.Map<IEnumerable<TReadDto>>(opItems));
        }
        // GET api/Model/{id} One use with id
        //[Authorize]
        [HttpGet("{id}", Name = "GetById")]
        public virtual async Task<ActionResult<TReadDto>> GetById(int id)
        {
            _logger.LogInformation(LogEvents.GetResourse, "Getting resourse {Id}", id);
            var entity = await _repository.GetByIdAsync(id);
            // if found return a user Dto
            if (entity == null)
            {
                _logger.LogWarning(LogEvents.GetResourseNotFound, "Get({Id}) NOT FOUND", id);
                return NotFound();
            }
            // make the repo get user usertypes to map it with user 
            return Ok(_mapper.Map<TReadDto>(entity));
        }

        // POST api/Model
        [Authorize]
        [HttpPost]
        public virtual async Task<ActionResult<TReadDto>> Create(TCreateDto CreateDto)
        {
            _logger.LogInformation(LogEvents.CreateResourse, "Creating new resourse");
            // Map user dto to use then create it in Db and save
            var entity = _mapper.Map<TModel>(CreateDto);
            var result = await _repository.CreateAsync(entity);
            if (!result.Success) return BadRequest(result.Message);
            await _repository.SaveChangesAsync();

            // need to send new URI with response, so map user to TReadDto response 
            // then pass the id of it to nameof GetGroupById and pass it to response as well
            var ReadDto = _mapper.Map<TReadDto>(result.Model);
            // the route for the resource using the get by id action
            var route = this.ControllerContext.ActionDescriptor.ControllerName + "/GetById";
            return CreatedAtRoute(route, new { ReadDto.Id }, ReadDto);
        }
        // PUT api/Model/{id}
        [Authorize]
        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Update(int id, TUpdateDto UpdateDto)
        {
            _logger.LogInformation(LogEvents.UpdateResourse, "Updating resourse {Id}", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning(LogEvents.GetResourseNotFound, "Get({Id}) NOT FOUND", id);
                return NotFound();
            }

            // Map user dto to user which will update Db then save
            _mapper.Map(UpdateDto, entity);
            var result = await _repository.UpdateAsync(entity);
            if (!result.Success) return BadRequest(result.Message);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // PATCH api/s/{id}
        [Authorize]
        [HttpPatch("{id}")]
        public virtual async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<TUpdateDto> patchDoc)
        {
            _logger.LogInformation(LogEvents.UpdateResourse, "Updating (patching) resourse {Id}", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning(LogEvents.GetResourseNotFound, "Get({Id}) NOT FOUND", id);
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
        [Authorize]
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation(LogEvents.DeleteResourse, "Deleting resourse {Id}", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning(LogEvents.GetResourseNotFound, "Get({Id}) NOT FOUND", id);
                return NotFound();
            }

            var result = await _repository.DeleteAsync(entity);
            if (!result.Success) return BadRequest(result.Message);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
        
    }
}