using AutoMapper;
using DeviceManagement.API.Dtos;
using DeviceManagement.API.Models;

namespace DeviceManagement.API.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceReadDto>();
            CreateMap<DeviceUpdateDto, Device>().ReverseMap();
            CreateMap<DeviceCreateDto, Device>();
        }
    }
}