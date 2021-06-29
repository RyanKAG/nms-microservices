using AutoMapper;
using Microsoft.Extensions.Logging;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;
using DeviceManagement.API.Repository;

namespace DeviceManagement.API.Controllers
{
    public class DeviceController : BaseControllerAsync<Device, DeviceReadDto, DeviceUpdateDto, DeviceCreateDto>
    {
        public DeviceController(IDeviceRepository repository, IMapper mapper, ILogger<Device> logger) : base(repository, mapper, logger)
        {
        }
    }
}