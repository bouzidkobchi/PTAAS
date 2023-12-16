using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;

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

//namespace WebApi.Controllers
//{
//    [Tags("authentication")]
//    [Route("api/auth")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IConfiguration _configuration;

//        public AuthController(UserManager<ApplicationUser> userManager,
//                              SignInManager<ApplicationUser> signInManager,
//                              IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _configuration = configuration;
//        }

//        /// <summary>
//        /// Authenticates a user and returns a JWT token
//        /// </summary>
//        /// <param name="model">The login view model that contains the user name and password</param>
//        /// <returns>A JSON object that contains the JWT token</returns>
//        [HttpPost("login")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        //[SwaggerOperation(OperationId = "Login", Summary = "Authenticates a user and returns a JWT token")]
//        //[SwaggerResponseHeader(StatusCodes.Status200OK, "Authorization", "string", "The JWT token")]
//        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
//        {
//            var user = await _userManager.FindByNameAsync(model.UserName);
//            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
//            {

//                var claims = new[]
//                {
//            new Claim(ClaimTypes.Name, user.UserName),
//            new Claim(ClaimTypes.Email, user.Email),
//            // Add additional claims as needed
//        };

//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//                var token = new JwtSecurityToken(
//                    _configuration["Jwt:Issuer"],
//                    _configuration["Jwt:Audience"],
//                    claims,

//                    expires: DateTime.Now.AddMinutes(30),
//                    signingCredentials: creds
//                );

//                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
//            }

//            return Unauthorized();
//        }

//        /// <summary>
//        /// Logs out the current user and returns a status code
//        /// </summary>
//        /// <returns>A 200 status code indicating the logout was successful</returns>
//        [HttpPost("logout")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> Logout()
//        {
//            // Perform the logout operation
//            await _signInManager.SignOutAsync();
//            // Return a 200 status code
//            return Ok();
//        }


//        /// <summary>
//        /// Registers a new user and returns a status code
//        /// </summary>
//        /// <param name="model">The register view model that contains the user name, email, and password</param>
//        /// <returns>A status code indicating the result of the registration</returns>
//        [HttpPost("register")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
//        {
//            // Check if the input is valid
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            // Create a new user object
//            var user = new Client
//            {
//                UserName = model.UserName,
//                Email = model.Email,
//            };

//            // Try to create the user in the database
//            var result = await _userManager.CreateAsync(user, model.Password);

//            // If the creation succeeded, return a 201 status code with the location of the user
//            if (result.Succeeded)
//            {
//                //return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
//                return Ok("created seccufuly !"); // wrong
//            }

//            // If the creation failed, return a 400 status code with the errors
//            return BadRequest(result.Errors);
//        }


//        /// <summary>
//        /// Refreshes the access token and returns a new token pair
//        /// </summary>
//        /// <param name="refreshToken">The refresh token that was issued with the previous access token</param>
//        /// <returns>A JSON object that contains the new access token and refresh token</returns>
//        //[HttpPost("refresh-token")]
//        //[ProducesResponseType(StatusCodes.Status200OK)]
//        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
//        //public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken)
//        //{
//        //    // Implement the token refresh logic here
//        //    // This could involve checking the refresh token, generating a new access token, and returning them to the client
//        //    // For example, using IdentityServer4:

//        //    // Validate the refresh token
//        //    var result = await _tokenService.CreateIdentityTokenAsync(refreshToken);

//        //    // If the refresh token is invalid, return a 400 status code with the error
//        //    if (result.IsError)
//        //    {
//        //        return BadRequest(result.Error);
//        //    }

//        //    // If the refresh token is valid, return a 200 status code with the new token pair
//        //    return Ok(new
//        //    {
//        //        accessToken = result.AccessToken,
//        //        refreshToken = result.RefreshToken
//        //    });
//        //}


//        [HttpGet("do-something")]
//        [Authorize]
//        public IActionResult DoSomething()
//        {
//            return Ok("loged In seccufuly !");
//        }

//        /// <summary>
//        /// not implemented
//        /// </summary>
//        //[HttpGet("users")]
//        //public IActionResult GetUsers()
//        //{
//        //    // Implement token refresh logic here if needed.
//        //    // This could involve checking a refresh token or generating a new access token.

//        //    //return Ok();
//        //    throw new NotImplementedException();
//        //}
//    }


//}

namespace WebApi.Controllers
{
    [Tags("authentication")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        //private readonly JWT _jwt;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration,
                              AuthService authService
,
                              RoleManager<IdentityRole> roleManager
/*JWT jwt*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _authService = authService;
            _roleManager = roleManager;
            //_jwt = jwt;
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

            var user = new Client
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

            //// default user role
            //var newRole = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() };

            //ArgumentNullException.ThrowIfNull(newRole);

            //await _roleManager.CreateAsync(newRole);

            await _userManager.AddToRoleAsync(user, "User");

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

        [Authorize(Roles = "User")]
        [HttpGet("for-only-users")]
        public string ForOnlyUsers()
        {
            return "hello world";
        }

        [HttpGet("how-am-i")]
        public IActionResult GetUserInfo()
        {
            var userInfo = JwtHelper.GetUserInfo(HttpContext);

            if (userInfo != null)
            {
                return Ok(userInfo);
            }
            else
            {
                return Unauthorized(); // or return a custom response based on your requirements
            }
        }

    }
}