using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Auth;
using WebApi.Auth.Extenions;
using WebApi.Auth.Helpers;
using WebApi.Auth.Services;
using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;

/*
 * POST /api/register
 * POST /api/login
 * POST /api/logout
 * POST /api/refresh-token
 * POST /api/forgot-password
 * POST /api/reset-password
 * POST /api/change-password
 * GET /api/user
 * PUT /api/user
 * 
 * 
 * nots : the admin is a pentester by the way !
 */

/*
    problem of reading user roles from the  database , and the jwt  creation 
    
    refresh token
    
 */

namespace WebApi.Controllers
{
    [Tags("authentication")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase , IAuthController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AuthService _authService;

        public AuthController(UserManager<ApplicationUser> userManager,
                              AuthService authService,
                              RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _authService = authService;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return BadRequest(new
                {
                    Message = "Email is already registered!",
                });

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return BadRequest(new
                {
                    Message = "Username is already registered!",
                });

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Failed to register this user!",
                    result.Errors,
                });
            }

            await _userManager.AddToRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Admin");

            var jwtSecurityToken = await _authService.CreateJwtToken(user);

            // register and login
            return Ok(new
            {
                user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest(new
                {
                    Message = "UserName or Password is incorrect!"
                });
            }


            var jwtSecurityToken = await _authService.CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = rolesList,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                // refresh token
                Username = user.UserName,
            });

        }

        [HttpPost("logout")]
        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

        [HttpPost("refresh-token")]
        public Task<IActionResult> RefreshToken()
        {
            throw new NotImplementedException();
        }

        [HttpPost("forgot-password")]
        public Task<IActionResult> ForgotPassword()
        {
            throw new NotImplementedException();
        }

        [HttpPost("reset-password")]
        public Task<IActionResult> ResetPassword()
        {
            throw new NotImplementedException();
        }

        [HttpPost("logout")]
        public Task<IActionResult> GetUserInfo()
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public Task<IActionResult> UpdateUserInfo()
        {
            throw new NotImplementedException();
        }

        [HttpPost("change-password")]
        public Task<IActionResult> ChangePassword()
        {
            throw new NotImplementedException();
        }
    }
}