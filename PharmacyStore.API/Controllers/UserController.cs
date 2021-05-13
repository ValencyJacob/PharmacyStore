using AutoMapper;
using BusinessLogic.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using PharmacyStore.API.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _loggerService;
        private readonly IConfiguration _configuration;

        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            ILoggerService loggerService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerService = loggerService;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>User registration</returns>
        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                var userName = userDTO.Email;
                var userPassword = userDTO.Password;

                var user = new IdentityUser
                {
                    Email = userName,
                    UserName = userName
                };

                var result = await _userManager.CreateAsync(user, userPassword);

                if (!result.Succeeded)
                {
                    _loggerService.LogError("User registration attempt failed.");

                    foreach(var error in result.Errors)
                    {
                        _loggerService.LogError($"{error.Code} - {error.Description}");
                    }

                    return StatusCode(500, "Server error.");
                }

                return Ok(new
                {
                    result.Succeeded
                });
            }
            catch(Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>User login</returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            try
            {
                var userName = userDTO.Email;
                var userPassword = userDTO.Password;

                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    var tokenString = await GenerateJSONWebToken(user);

                    return Ok(new { token = tokenString});
                }

                _loggerService.LogWarn($"{userName} not authenticated!");

                return Unauthorized(userDTO);
            }
            catch(Exception ex)
            {
                _loggerService.LogError($"{ex.Message}");
                return Unauthorized(userDTO);
            }
        }
        
        private async Task<string> GenerateJSONWebToken(IdentityUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(x => new Claim(ClaimsIdentity.DefaultRoleClaimType, x)));

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], claims, null, 
                expires: DateTime.Now.AddDays(21), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
