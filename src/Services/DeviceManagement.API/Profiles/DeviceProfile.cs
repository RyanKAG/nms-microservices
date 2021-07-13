using AutoMapper;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;
using Event.Messages.Events.DeviceEvents;

namespace DeviceManagement.API.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceReadDto>();
            CreateMap<DeviceUpdateDto, Device>().ReverseMap();
            CreateMap<DeviceCreateDto, Device>();
            CreateMap<Device, DeviceDeletedEvent>();
            CreateMap<Device, DeviceUpdatedEvent>();
            CreateMap<Device, DeviceCreatedEvent>();
        }
    }
}