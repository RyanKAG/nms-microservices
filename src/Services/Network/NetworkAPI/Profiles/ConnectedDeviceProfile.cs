using AutoMapper;
using Event.Messages.Events.DeviceEvents;
using NetworkAPI.EventBusConsumer;
using NetworkAPI.Models;

namespace NetworkAPI.Profiles
{
    public class ConnectedDeviceProfile : Profile
    {
        public ConnectedDeviceProfile()
        {
            CreateMap<DeviceCreatedEvent, Device>();
            CreateMap<DeviceUpdatedEvent, Device>().ReverseMap();
            CreateMap<DeviceDeletedEvent, Device>();
        }
    }
}