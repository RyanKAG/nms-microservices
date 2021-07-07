using AutoMapper;
using Auth.API.Dtos;
using Auth.API.Models;

namespace Auth.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>();

        }
    }
}