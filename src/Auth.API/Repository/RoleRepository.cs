using System;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Repository
{
    public class RoleRepository : BaseRepo<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}