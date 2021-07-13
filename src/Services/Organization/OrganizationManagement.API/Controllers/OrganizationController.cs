using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationManagement.API.Dtos;
using OrganizationManagement.API.Models;
using OrganizationManagement.API.Repository;

namespace OrganizationManagement.API.Controllers
{
    public class OrganizationController: BaseControllerAsync<Organization, OrganizationReadDto, OrganizationUpdateDto, OrganizationCreateDto>
    {
        public OrganizationController(IOrganizationRepository repository, IMapper mapper, ILogger<Organization> logger) : base(repository, mapper, logger)
        {
        }
        
        [HttpPost("{id}/network/{networkId}")]
        public async Task<IActionResult> AddNetworkToorg(Guid id, Guid networkId)
        {
            //Down cast Irepo to INetworkRepo
            var repo = (IOrganizationRepository) Repository;
            var org = await repo.GetByIdAsync(id);
            if (org == null)
                return NotFound($"Organization with ID: {id} does not exist");

            var network = await repo.GetNetworkById(networkId);
            if(network == null)
                return NotFound($"Network with ID: {networkId} does not exist");

            repo.AddNetworkToOrg(org, network);
            var isSaved = await Repository.SaveChangesAsync();
            
            if (!isSaved)
                return Problem(statusCode: 500);
            
            return Ok();
        }
    }
}