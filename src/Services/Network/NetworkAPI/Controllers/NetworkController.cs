using AutoMapper;
using Microsoft.Extensions.Logging;
using NetworkAPI.Dtos;
using NetworkAPI.Models;
using NetworkAPI.Repository;

namespace NetworkAPI.Controllers
{
    public class NetworkController : BaseControllerAsync<Network, NetworkReadDto, NetworkUpdateDto, NetworkCreateDto>
    {
        public NetworkController(IRepository<Network> repository, IMapper mapper, ILogger<Network> logger) : base(repository, mapper, logger)
        {
            
        }
    }
}