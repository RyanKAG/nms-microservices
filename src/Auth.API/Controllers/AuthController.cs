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
        private readonly SignInManager<User> _signInManager;


        public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IConfiguration config, ITokenManager tokenManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _config = config;
            _tokenManager = tokenManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
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

            //create the role if role doesn't exist
            if (role == null)
               await _roleManager.CreateAsync(new Role(Role.Admin));
            await _userManager.AddToRoleAsync(user, Role.Admin);
            
            //TODO: Change the return to CreatedAt
            return Ok();
            
        }

        [HttpPost("login")]
        public async Task<ActionResult> login(LoginDto loginDto)
        {
            if (loginDto.Email == null && loginDto.UserName == null)
            {
                return BadRequest("At least Email or Username has to be sent");
            }

            User user = null;
            
            if (loginDto.Email != null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.Email);
            }
            else if(loginDto.UserName != null)
            {
                user = await _userManager.FindByNameAsync(loginDto.UserName);
            }

            if (user == null)
                return Unauthorized();


            var isAuthenticated = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            var roles = await _userManager.GetRolesAsync(user);
            if (isAuthenticated)
            {
                var key = _config["JWT:Key"];
                var jwtToken = _tokenManager.GenerateJwt(key, _config["JWT:Issuer"], user, roles);
                return Ok(new{token = jwtToken});
            }

            return Unauthorized();
        }
    }
}