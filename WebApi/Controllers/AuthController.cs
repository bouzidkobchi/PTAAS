using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.UserName);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var claims = new[]
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(ClaimTypes.Email, user.Email),
        //            // Add additional claims as needed
        //        };

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(
        //            _configuration["Jwt:Issuer"],
        //            _configuration["Jwt:Issuer"],
        //            claims,
        //            expires: DateTime.Now.AddMinutes(30),
        //            signingCredentials: creds
        //        );

        //        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        //    }

        //    return Unauthorized();
        //}

        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok();
        //}

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        //{
        //    var user = new ApplicationUser
        //    {
        //        UserName = model.UserName,
        //        Email = model.Email,
        //        // Add additional user properties as needed
        //    };

        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    if (result.Succeeded)
        //    {
        //        // You may choose to sign in the user after registration, or send a confirmation email, etc.
        //        // For simplicity, we'll just return a success message.
        //        return Ok(new { Message = "User registered successfully" });
        //    }

        //    return BadRequest(result.Errors);
        //}

        //[HttpPost("refresh-token")]
        //public IActionResult RefreshToken()
        //{
        //    // Implement token refresh logic here if needed.
        //    // This could involve checking a refresh token or generating a new access token.

        //    return Ok();
        //}
    }

    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
