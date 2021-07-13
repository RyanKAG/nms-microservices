using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.API.Dtos;
using OrganizationManagement.API.Models;

namespace OrganizationManagement.API.Profiles
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<OrganizationCreateDto, Organization>();
            CreateMap<OrganizationUpdateDto, Organization>().ReverseMap();
            CreateMap<Organization, OrganizationReadDto>().ReverseMap();
        }
        
    }
}