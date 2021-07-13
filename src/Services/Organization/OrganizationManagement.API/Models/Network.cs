using System;

namespace OrganizationManagement.API.Models
{
    public class Network
    {
        public Guid Id { get; set; }
        public Organization Organization { get; set; }
    }
}