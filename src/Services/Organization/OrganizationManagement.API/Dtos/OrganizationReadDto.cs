using System;
using System.Collections.Generic;

namespace OrganizationManagement.API.Dtos
{
    public class OrganizationReadDto :  IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public IEnumerable<Guid> Networks { get; set; }

    }
}