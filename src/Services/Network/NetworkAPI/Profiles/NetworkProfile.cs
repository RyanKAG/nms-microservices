using AutoMapper;
using Event.Messages.Events.NetworkEvents;
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
            CreateMap<Network, NetworkCreatedEvent>();
        }
    }
}