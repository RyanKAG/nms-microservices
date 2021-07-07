using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Models
{
    public class Role : IdentityRole<Guid>, IModel
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public static readonly string Admin = "Admin";
        public static readonly string User = "User";
        public static readonly string Guest = "Guest";
    }
}