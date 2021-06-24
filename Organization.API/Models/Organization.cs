using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organization.API.Models
{
    public class Organization : IModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        //Not yet implemented
        //  [NotMapped]
        // public List<> Type { get; set; }
        
    }
}