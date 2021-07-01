using AutoMapper;
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
    }
}