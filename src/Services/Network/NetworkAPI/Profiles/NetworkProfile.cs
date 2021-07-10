using AutoMapper;
using NetworkAPI.Dtos;
using NetworkAPI.Models;

namespace NetworkAPI.Profiles
{
    public class NetworkProfile : Profile
    {
        public NetworkProfile()
        {
            CreateMap<Network, NetworkReadDto>();
            CreateMap<NetworkCreateDto, Network>();
            CreateMap<NetworkUpdateDto, Network>().ReverseMap();
        }
    }
}