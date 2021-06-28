using AutoMapper;
using Microsoft.Extensions.Logging;
using DeviceAPI.Dtos;
using DeviceAPI.Models;
using DeviceAPI.Repository;

namespace DeviceAPI.Controllers
{
    public class DeviceController : BaseControllerAsync<Device, DeviceReadDto, DeviceUpdateDto, DeviceCreateDto>
    {
        public DeviceController(IDeviceRepository repository, IMapper mapper, ILogger<Device> logger) : base(repository, mapper, logger)
        {
        }
    }
}