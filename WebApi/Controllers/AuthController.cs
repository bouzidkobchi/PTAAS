using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Auth;
using WebApi.Auth.Extenions;
using WebApi.Auth.Models;
using WebApi.Auth.Services;
using WebApi.Data;
using WebApi.Models;

/*
 * POST /api/register       done
 * POST /api/login          done
 * POST /api/logout         done
 * POST /api/refresh-token  done
 * POST /api/forgot-password    done
 * POST /api/reset-password     done
 * POST /api/change-password    done
 * GET /api/user                done
 * PUT /api/user                done
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
        private readonly AppDbContext _dbContext;
        private readonly JwtService _jwtService;
        //private readonly IEmailSender<ApplicationUser> _emailService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private static string[] _defaultRoles = new[] {"Client"};

        public AuthController(UserManager<ApplicationUser> userManager,
                              JwtService authService,
                              SignInManager<ApplicationUser> signInManager,
                              AppDbContext dbContext,
                              //IEmailSender<ApplicationUser> emailService,
                              JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _userManager = userManager;
            _jwtService = authService;
            _signInManager = signInManager;
            _dbContext = dbContext;
            //_emailService = emailService;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        //[HttpGet("send-email")]
        //public async Task<IActionResult> sendEmail(string email, string subject , string message, int port, string domain)
        //{
        //    await new EmailSender().SendMessage(email, subject, message,port,domain);

        //    return Ok(new
        //    {
        //        message = "email sent sucessfuly !"
        //    });
        //}

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
            foreach (var role in _defaultRoles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return Ok(new
            {
                Message = "User Created Seccessfuly."
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
                    Message = "Email or Password is incorrect!"
                });
            }

            // Create a JWT token for the logged-in user
            var jwtAccessToken = await _jwtService.CreateAccessToken(user);
            var jwtRefreshToken = _jwtService.CreateRefreshToken(user);

            // Get the roles assigned to the user
            var rolesList = await _userManager.GetRolesAsync(user);

            // Return information about the logged-in user and the JWT token
            return Ok(new
            {
                user.Email,
                AccessTokenExpiresOn = jwtAccessToken.ValidTo,
                RefreshTokenExpiresOn = jwtRefreshToken.ValidTo,
                Roles = rolesList,
                AccessToken = _jwtSecurityTokenHandler.WriteToken(jwtAccessToken),
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

        /// <summary>
        /// Initiates the password reset process for a user by sending a reset password link to their email.
        /// </summary>
        /// <param name="email">The email address of the user requesting a password reset.</param>
        /// <returns>An IActionResult representing the result of the password reset initiation.</returns>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword(string email) // not completed
        {
            try
            {
                // Retrieve the user by email
                var user = await _userManager.FindByEmailAsync(email);

                // Check if the user exists
                if (user == null)
                {
                    // Handle user not found
                    return NotFound(new
                    {
                        Message = $"User with email = {email} not found."
                    });
                }

                // Generate a reset password token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Check if the token was successfully generated
                if (resetToken == null)
                {
                    return BadRequest(new
                    {
                        Message = "Failed to generate reset password token."
                    });
                }

                // Send reset password URL to the user's email with the email and the token
                //_emailService.SendResetPasswordEmail(user.Email, resetToken); // Implement this method in your email service

                return Ok(new
                {
                    Message = "Reset password URL sent successfully."
                });
            }
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new
                {
                    Message = "Internal server error.",
                    //Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Resets a user's password using a reset token.
        /// </summary>
        /// <param name="model">The reset password model containing user ID, new password, and reset token.</param>
        /// <returns>An IActionResult representing the result of the password reset operation.</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([Required , FromBody] ResetPasswordModel model)
        {
            try
            {
                // Retrieve the user by Id
                var user = await _userManager.FindByIdAsync(model.UserId);

                // Check if the user exists
                if (user == null)
                {
                    // Handle user not found
                    return NotFound(new
                    {
                        Message = $"User of ID = {model.UserId} not found."
                    });
                }

                // Reset the password using the provided reset token
                var result = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);

                // Return the result of the password reset operation
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Message = "Password reset successful."
                    });
                }

                return BadRequest(new
                {
                    Message = "Failed to reset password.",
                    result.Errors
                });
            }
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new
                {
                    Message = "Internal server error.",
                    //Error = ex.Message
                });
            }
        }

        // <summary>
        /// Retrieves information about the authenticated user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// /api/users/user-info
        /// </remarks>
        /// <returns>Returns Ok with information about the authenticated user.</returns>
        [Authorize]
        [HttpGet("my-info")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful retrieval
        [ProducesResponseType(typeof(object), 401)] // Swagger response type for unauthorized
        [ProducesResponseType(typeof(object), 500)] // Swagger response type for server error
        public IActionResult GetMyInfo()
        {
            try
            {
                // Retrieve information about the authenticated user using an extension method
                var userInfo = User.GetUserInfo();

                // Return information about the authenticated user
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new
                {
                    Message = "Internal server error.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Changes the password for the authenticated user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/users/change-password
        /// {
        ///   "currentPassword": "CurrentPassword123",
        ///   "newPassword": "NewPassword456"
        /// }
        /// </remarks>
        /// <param name="model">The change password model.</param>
        /// <returns>Returns Ok if the password is successfully changed, BadRequest if the user is not found or the change password operation fails.</returns>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful password change
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 401)] // Swagger response type for unauthorized
        [ProducesResponseType(typeof(object), 500)] // Swagger response type for server error
        public async Task<IActionResult> ChangePassword(changePasswordModel model)
        {
            try
            {
                // Retrieve the user from the claims
                var user = User.LoadUser(_dbContext);

                // Check if the user exists
                if (user == null)
                {
                    return BadRequest(new
                    {
                        Message = "User not found."
                    });
                }

                // Change the user's password
                var result = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);

                // Check if the password change operation was successful
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Message = "Password changed successfully."
                    });
                }

                // Return appropriate error message for password change failure
                return BadRequest(new
                {
                    Message = "Failed to change password.",
                    result.Errors
                });
            }
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new
                {
                    Message = "Internal server error.",
                    //Error = ex.Message
                }) ;
            }
        }


        /// <summary>
        /// Endpoint to refresh an access token using a valid refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token used for obtaining a new access token.</param>
        /// <returns>
        /// Returns a new access token along with relevant user information if the refresh token is valid.
        /// Otherwise, returns a Bad Request with an error message.
        /// </returns>
        [HttpGet("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                // Read and validate the refresh token
                var userClaimsPrincipal = _jwtService.ReadToken(refreshToken, out var jwtSecurityToken);

                // Check if the refresh token is invalid
                if (userClaimsPrincipal == null || jwtSecurityToken == null)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid refresh token."
                    });
                }

                // Load user from the database using the refresh token
                var user = userClaimsPrincipal.LoadUser(_dbContext);

                // Check if the user is not found
                if (user == null)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid refresh token."
                    });
                }

                // Create a new access token
                var jwtAccessToken = await _jwtService.CreateAccessToken(user);

                // Return successful response with user information and tokens
                return Ok(new
                {
                    user.Email,
                    AccessTokenExpiresOn = jwtAccessToken.ValidTo,
                    RefreshTokenExpiresOn = jwtSecurityToken.ValidTo,
                    Roles = userClaimsPrincipal.GetRoles(),
                    AccessToken = _jwtSecurityTokenHandler.WriteToken(jwtAccessToken),
                    RefreshToken = refreshToken,
                    Username = user.UserName,
                });
            }
            catch (Exception)
            {
                // Log the exception for further investigation

                // Return a generic error response
                return BadRequest(new
                {
                    Message = "An error occurred while refreshing the token. Please try again later."
                });
            }
        }
    }
}