using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApi.Auth.Models;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase , IUsersController
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UsersController(AppDbContext dbContext, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// {
        ///   "userName": "JohnDoe",
        ///   "email": "john.doe@example.com",
        ///   "password": "StrongPassword123"
        /// }
        /// </remarks>
        /// <param name="model">The user model.</param>
        /// <returns>Returns the ID of the created user.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful creation
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 500)] // Swagger response type for server error
        public async Task<IActionResult> CreateUserAsync([FromBody] UserModel model)
        {
            try
            {
                // Create an ApplicationUser instance from the UserModel properties
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    // Add other properties as needed
                };

                // Save the user to the database
                var result = await _userManager.CreateAsync(user, model.Password);

                // Check if user creation was successful
                if (result.Succeeded)
                {
                    // Return the ID of the created user
                    return Ok(new
                    {
                        UserId = user.Id
                    });
                }

                // Return errors if user creation failed
                return BadRequest(new
                {
                    Message = "Failed to create user.",
                    Errors = result.Errors
                });
            }
            catch (Exception ex)
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
        /// Gets a page of users.
        /// </summary>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The page size (default is 10).</param>
        /// <returns>Returns a page of users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful retrieval
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        public IActionResult GetUsersPage(int pageNumber = 1, int pageSize = 10)
        {
            // Validate page number and page size
            if (pageNumber < 1 || pageSize < 1 || pageSize > 10)
            {
                return BadRequest(new
                {
                    Message = "Page number and page size should be positive, and page size should be between 1 and 10."
                });
            }

            // Retrieve a page of users, don't forget to include roles
            var users = _dbContext.Users
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Return the page of users
            return Ok(users);
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>Returns the user if found, or NotFound if not found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful retrieval
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        public IActionResult GetUser(string id)
        {
            var user = _dbContext.Users.Find(id);

            // Check if the user exists
            if (user == null)
            {
                return NotFound(new
                {
                    Message = $"There is no user with ID = {id}."
                });
            }

            // Return the user
            return Ok(user);
        }

        /// <summary>
        /// Adds a user to a specified role.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// /api/users/AddToRole?id=UserId&roleName=RoleName
        /// </remarks>
        /// <param name="id">The ID of the user.</param>
        /// <param name="roleName">The name of the role to add the user to.</param>
        /// <returns>Returns Ok if the user is successfully added to the role, NotFound if the user or role is not found, or BadRequest for invalid input.</returns>
        [HttpPost("AddToRole")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful operation
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        [ProducesResponseType(typeof(object), 500)] // Swagger response type for server error
        public async Task<IActionResult> AddToRoleAsync(string id, string roleName)
        {
            try
            {
                // Retrieve the user by ID
                var user = await _userManager.FindByIdAsync(id);

                // Check if the user exists
                if (user == null)
                {
                    return NotFound(new
                    {
                        Message = $"User with ID = {id} not found."
                    });
                }

                // Retrieve the role by name
                var role = await _roleManager.FindByNameAsync(roleName);

                // Check if the role exists
                if (role == null)
                {
                    return NotFound(new
                    {
                        Message = $"Role with name = {roleName} not found."
                    });
                }

                // Add the user to the role
                var result = await _userManager.AddToRoleAsync(user, roleName);

                // Check if the user was successfully added to the role
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Message = $"User with ID = {id} added to role {roleName} successfully."
                    });
                }

                // Return errors if the operation failed
                return BadRequest(new
                {
                    Message = "Failed to add user to role.",
                    Errors = result.Errors
                });
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
        /// Updates user information (excluding the password).
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// /api/users?id=UserId
        /// {
        ///   "userName": "NewUserName",
        ///   "email": "new.email@example.com"
        /// }
        /// </remarks>
        /// <param name="model">The user model containing updated information.</param>
        /// <param name="userId">The ID of the user to update.</param>
        /// <returns>Returns Ok if the user information is successfully updated, NotFound if the user is not found, or BadRequest for invalid input.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful update
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        [ProducesResponseType(typeof(object), 500)] // Swagger response type for server error
        public async Task<IActionResult> UpdateUserInfo_WithoutThePasswordAsync([Required] UserModel model, [Required, FromRoute] string userId)
        {
            try
            {
                // Retrieve the user by ID
                var user = await _userManager.FindByIdAsync(userId);

                // Check if the user exists
                if (user == null)
                {
                    return NotFound(new
                    {
                        Message = $"User with ID = {userId} not found."
                    });
                }

                // Update user information
                user.Email = model.Email;
                user.UserName = model.UserName;

                // Save the updated user information to the database
                var result = await _userManager.UpdateAsync(user);

                // Check if the update was successful
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Message = $"User with ID = {userId} information updated successfully."
                    });
                }

                // Return errors if the update failed
                return BadRequest(new
                {
                    Message = "Failed to update user information.",
                    Errors = result.Errors
                });
            }
            catch (Exception ex)
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
        /// Removes a user from a specified role.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// /api/users/RemoveFromRole?id=UserId&roleName=RoleName
        /// </remarks>
        /// <param name="id">The ID of the user.</param>
        /// <param name="roleName">The name of the role to remove the user from.</param>
        /// <returns>Returns Ok if the user is successfully removed from the role, NotFound if the user or role is not found, or BadRequest for invalid input.</returns>
        [HttpPost("RemoveFromRole")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful operation
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        [ProducesResponseType(typeof(object), 500)] // Swagger response type for server error
        public async Task<IActionResult> RemoveFromRoleAsync([FromQuery] string id, [FromQuery] string roleName)
        {
            try
            {
                // Retrieve the user by ID
                var user = await _userManager.FindByIdAsync(id);

                // Check if the user exists
                if (user == null)
                {
                    return NotFound(new
                    {
                        Message = $"User with ID = {id} not found."
                    });
                }

                // Retrieve the role by name
                var role = await _roleManager.FindByNameAsync(roleName);

                // Check if the role exists
                if (role == null)
                {
                    return NotFound(new
                    {
                        Message = $"Role with name = {roleName} not found."
                    });
                }

                // Remove the user from the role
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                // Check if the user was successfully removed from the role
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        Message = $"User with ID = {id} removed from role {roleName} successfully."
                    });
                }

                // Return errors if the operation failed
                return BadRequest(new
                {
                    Message = "Failed to remove user from role.",
                    Errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new
                {
                    Message = "Internal server error.",
                    //Error = ex.Message 
                });
            }
        }
    }
}
