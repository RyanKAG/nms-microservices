
using MassTransit;
using System;
using System.Threading.Tasks;
using Event.Messages.Events.DeviceEvents;
using NetworkAPI.Models;
using NetworkAPI.Repository;

namespace NetworkAPI.EventBusConsumer
{
    public class DeviceDeletedConsumer : IConsumer<DeviceDeletedEvent>
    {
        private readonly INetworkRepository _networkRepo;

        public DeviceDeletedConsumer(INetworkRepository networkRepo)
        {
            _networkRepo = networkRepo;
        }

        public  Task Consume(ConsumeContext<DeviceDeletedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}