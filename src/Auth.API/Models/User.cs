using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Models
{
    public class User : IdentityUser<Guid>, IModel
    {
        [Phone]
        public string Phone { get; set; }
        
    }
}