using System;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
         
    }
}