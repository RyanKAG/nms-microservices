using AutoMapper;
using Microsoft.Extensions.Logging;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;
using DeviceManagement.API.Repository;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Event.Messages.Events.DeviceEvents;

namespace DeviceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : BaseControllerAsync<
        Device,
        DeviceReadDto,
        DeviceUpdateDto,
        DeviceCreateDto,
        DeviceCreatedEvent,
        DeviceDeletedEvent,
        DeviceUpdatedEvent>
    {
        public DeviceController(IDeviceRepository repository, IMapper mapper, ILogger<Device> logger,
            IPublishEndpoint pub) : base(repository, mapper, logger, pub)
        {
        }
        
    }
}