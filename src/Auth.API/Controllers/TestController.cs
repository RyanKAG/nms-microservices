using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Auth.API.Models;
using Auth.API.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IRoleRepository _repo;
        private readonly UserManager<User> _userManager;

        public TestController(IRoleRepository repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> Get()
        {
            var users = await  _userManager.Users.ToListAsync();
            return Ok(users);
        }
    }
}