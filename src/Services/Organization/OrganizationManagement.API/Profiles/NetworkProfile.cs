using AutoMapper;
using Event.Messages.Events.NetworkEvents;
using OrganizationManagement.API.Models;

namespace OrganizationManagement.API.Profiles
{
    public class NetworkProfile : Profile
    {
        public NetworkProfile()
        {
            CreateMap<NetworkCreatedEvent, Network>();
        }
    }
}