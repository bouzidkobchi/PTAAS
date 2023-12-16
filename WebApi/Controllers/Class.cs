//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using WebApi.Models;

//namespace WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TokenController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;

//        public TokenController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        /// <summary>
//        /// Authenticates a user and returns a JWT token and a refresh token
//        /// </summary>
//        /// <param name="model">The login view model that contains the user name and password</param>
//        /// <returns>A JSON object that contains the JWT token and the refresh token</returns>
//        [HttpPost("login")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        //[SwaggerOperation(OperationId = "Login", Summary = "Authenticates a user and returns a JWT token and a refresh token")]
//        //[SwaggerResponseHeader(StatusCodes.Status200OK, "Authorization", "string", "The JWT token")]
//        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
//        {
//            var user = await _userManager.FindByNameAsync(model.UserName);
//            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
//            {

//                var claims = new[]
//                {
//                    new Claim(ClaimTypes.Name, user.UserName),
//                    new Claim(ClaimTypes.Email, user.Email),
//                    // Add additional claims as needed
//                };

//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//                // Generate a JWT token
//                var token = new JwtSecurityToken(
//                    _configuration["Jwt:Issuer"],
//                    _configuration["Jwt:Audience"],
//                    claims,
//                    expires: DateTime.Now.AddMinutes(30),
//                    signingCredentials: creds
//                );

//                // Generate a refresh token
//                var refreshToken = GenerateRefreshToken();

//                // Save the refresh token in a database or a cache
//                // TODO: Implement your own logic to store the refresh token

//                // Return the JWT token and the refresh token
//                return Ok(new { 
//                    token = new JwtSecurityTokenHandler().WriteToken(token),
//                    refreshToken = refreshToken
//                });
//            }

//            return Unauthorized();
//        }

//        private string GenerateRefreshToken()
//        {
//            // Create a random string as the refresh token
//            // TODO: You can use more secure ways to generate the refresh token
//            var random = new Random();
//            var refreshToken = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 32)
//              .Select(s => s[random.Next(s.Length)]).ToArray());

//            // Set the expiration time of the refresh token
//            // TODO: You can store the expiration time along with the refresh token
//            var expiration = DateTime.Now.AddDays(7);

//            return refreshToken;
//        }
//    }
//}
