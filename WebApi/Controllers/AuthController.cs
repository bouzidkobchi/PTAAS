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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtService _jwtService;
        private readonly string[] _defaultRoles = new[] {"Client"};

        public AuthController(UserManager<ApplicationUser> userManager,
                              JwtService authService,
                              RoleManager<ApplicationRole> roleManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _jwtService = authService;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration model.</param>
        /// <returns>Returns information about the registered user and a JWT token.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type
        [ProducesResponseType(typeof(object), 400)] // Swagger response type
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Check if the email is already registered
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return BadRequest(new
                {
                    Message = "Email is already registered!",
                });

            // Check if the username is already registered
            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return BadRequest(new
                {
                    Message = "Username is already registered!",
                });

            // Create a new ApplicationUser
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, model.Password);

            // Check if user creation was successful
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Failed to register this user!",
                    result.Errors,
                });
            }

            // Add default roles to the user
            await _userManager.AddToRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Admin");

            foreach (var role in _defaultRoles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            // Create a JWT token for the registered user
            var jwtSecurityToken = await _jwtService.CreateToken(user);

            // Return information about the registered user and the JWT token
            return Ok(new
            {
                user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = _defaultRoles,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
            });
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="model">The login model.</param>
        /// <returns>Returns information about the logged-in user and a JWT token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type
        [ProducesResponseType(typeof(object), 400)] // Swagger response type
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Check if the user exists and the password is correct
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest(new
                {
                    Message = "UserName or Password is incorrect!"
                });
            }

            // Create a JWT token for the logged-in user
            var jwtSecurityToken = await _jwtService.CreateToken(user);

            // Get the roles assigned to the user
            var rolesList = await _userManager.GetRolesAsync(user);

            // Return information about the logged-in user and the JWT token
            return Ok(new
            {
                user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = rolesList,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                // Add any additional information, e.g., refresh token
                Username = user.UserName,
            });
        }

        /// <summary>
        /// Logs out the authenticated user.
        /// </summary>
        /// <returns>Returns a message indicating successful logout.</returns>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await _signInManager.SignOutAsync();

            // Return a successful logout message
            return Ok(new
            {
                Message = "User successfully logged out."
            });
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

        [Authorize]
        [HttpPost("user-info")]
        public IActionResult GetUserInfo()
        {
            return Ok(User.GetUserInfo());
        }

        [HttpPost("{id}")]
        public Task<IActionResult> UpdateUserInfo()
        {
            throw new NotImplementedException();
            // return all properties including roles , tests , and other things
        }

        [HttpPost("change-password")]
        public Task<IActionResult> ChangePassword()
        {
            throw new NotImplementedException();
        }
    }
}