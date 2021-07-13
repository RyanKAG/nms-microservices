using System.Threading.Tasks;
using AutoMapper;
using Event.Messages.Events.DeviceEvents;
using MassTransit;
using NetworkAPI.Models;
using NetworkAPI.Repository;

namespace NetworkAPI.EventBusConsumer
{
    public class DeviceCreatedConsumer : IConsumer<DeviceCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly INetworkRepository _networkRepo;

        public DeviceCreatedConsumer(IMapper mapper, INetworkRepository networkRepo)
        {
            _mapper = mapper;
            _networkRepo = networkRepo;
        }
        public async Task Consume(ConsumeContext<DeviceCreatedEvent> context)
        {
            var device = _mapper.Map<Device>(context.Message);
            await _networkRepo.AddDevice(device);
            await _networkRepo.SaveChangesAsync();
        }
    }
}