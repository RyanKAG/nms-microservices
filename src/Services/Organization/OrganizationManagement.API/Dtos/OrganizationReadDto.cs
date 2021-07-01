using System;

namespace OrganizationManagement.API.Dtos
{
    public class OrganizationReadDto :  IDto
    {
        public Guid Id { get; set; }
    }
}