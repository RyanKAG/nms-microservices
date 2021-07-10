using AutoMapper;
using Microsoft.Extensions.Logging;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;
using DeviceManagement.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : BaseControllerAsync<Device, DeviceReadDto, DeviceUpdateDto, DeviceCreateDto>
    {
        public DeviceController(IDeviceRepository repository, IMapper mapper, ILogger<Device> logger) : base(repository, mapper, logger)
        {
        }
    }
}