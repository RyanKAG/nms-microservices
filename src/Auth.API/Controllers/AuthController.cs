using System;
using System.Linq;
using System.Threading.Tasks;
using Auth.API.Models;
using Auth.API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Auth.API.Dtos;
using Auth.API.Utils.JWT;
using Microsoft.Extensions.Configuration;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ITokenManager _tokenManager;
        public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("/register")]
        public async Task<ActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            var role = await _roleManager.FindByNameAsync(Role.Admin);
            var emailExits = await _userManager.FindByEmailAsync(registerDto.Email) != null;
            var usernameExists = await _userManager.FindByNameAsync(registerDto.UserName) != null;
            var errors = new List<string>();
            
            if (usernameExists)
            {
                errors.Add("Username already in use");
            }
            if (emailExits)
            {
                errors.Add("Email already in use");
            }
            
            if (errors.Count != 0)
                return Conflict(errors);
            var user = _mapper.Map<User>(registerDto);
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (role == null)
               await _roleManager.CreateAsync(new Role(Role.Admin));
            await _userManager.AddToRoleAsync(user, Role.Admin);
            return Ok();
            
        }
    }
}