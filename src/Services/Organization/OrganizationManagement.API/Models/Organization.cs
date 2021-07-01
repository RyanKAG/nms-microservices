using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationManagement.API.Models
{
    public class Organization : BaseModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        
        
    }
}