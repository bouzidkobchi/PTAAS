using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApi.Auth.Models;
using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase , IRoleController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _dbContext;

        public RoleController(RoleManager<ApplicationRole> roleManager, AppDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="model">The role model.</param>
        /// <returns>Returns the created role if successful, or BadRequest with errors if unsuccessful.</returns>
        [HttpPost("CreateRole")]
        [ProducesResponseType(typeof(ApplicationRole), 201)] // Swagger response type for successful creation
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        public async Task<IActionResult> CreateRole([Required] RoleModel model)
        {
            // Create a new ApplicationRole
            var role = new ApplicationRole
            {
                Name = model.Name,
                Permissions = model.Permissions,
            };

            // Attempt to create the role
            var result = await _roleManager.CreateAsync(role);

            // Check if role creation was successful
            if (result.Succeeded)
            {
                // Return a 201 Created response with the created role
                return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
            }

            // Return a BadRequest response with the errors if role creation failed
            return BadRequest(new
            {
                Message = "Failed to create role.",
                result.Errors,
            });
        }

        /// <summary>
        /// Deletes a role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>Returns Ok with a success message if the role is deleted, or NotFound if the role does not exist. 
        /// Returns BadRequest with errors if the deletion fails.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful deletion
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        public async Task<IActionResult> DeleteRole([Required] string id)
        {
            // Retrieve the role by ID
            var role = await _roleManager.FindByIdAsync(id);

            // Check if the role exists
            if (role == null)
            {
                // Return a 404 Not Found response with a custom message
                return NotFound(new
                {
                    Message = $"There is no role with ID = {id}."
                });
            }

            // Attempt to delete the role
            var result = await _roleManager.DeleteAsync(role);

            // Check if the deletion was successful
            if (result.Succeeded)
            {
                // Return a 200 OK response with a success message
                return Ok(new
                {
                    Message = $"Role with ID = {id} was deleted successfully."
                });
            }

            // Return a BadRequest response with errors if the deletion failed
            return BadRequest(new
            {
                Message = $"Failed to delete role with ID = {id}.",
                result.Errors
            });
        }

        /// <summary>
        /// Gets a role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>Returns the role if found, or NotFound if the role does not exist.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationRole), 200)] // Swagger response type for successful retrieval
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        public async Task<IActionResult> GetRoleById([Required] string id)
        {
            // Retrieve the role by ID
            var role = await _roleManager.FindByIdAsync(id);

            // Check if the role exists
            if (role == null)
            {
                // Return a 404 Not Found response with a custom message
                return NotFound(new
                {
                    Message = $"There is no role with ID = {id}."
                });
            }

            // Return the role if found
            return Ok(role);
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>Returns a list of all roles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful retrieval
        public IActionResult GetRoles()
        {
            // Retrieve all roles from the database
            var roles = _dbContext.Roles
                    .AsNoTracking()
                    .ToList();

            // Return the list of roles
            return Ok(roles);
        }

        //public IActionResult GetRolesWithRoles()
        //{
        //    var usersWithRoles = _dbContext.Users
        //.Join(
        //    _dbContext.UserRoles,
        //    user => user.Id,
        //    userRole => userRole.UserId,
        //    (user, userRole) => new
        //    {
        //        UserId = user.Id,
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        Role = userRole.Role.Name // Assuming there's a navigation property 'Role' in UserRoles
        //    }
        //)
        //.ToList();

        //    return Ok(usersWithRoles);
        //}

        /// <summary>
        /// Gets all available permissions.
        /// </summary>
        /// <returns>Returns a list of all available permissions.</returns>
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful retrieval
        public IActionResult GetPermissions()
        {
            // Assuming Permission is an enum type
            var permissions = Enum.GetNames(typeof(Permission));

            // Return the list of permissions
            return Ok(permissions);
        }

        /// <summary>
        /// Updates a role by its ID.
        /// </summary>
        /// <param name="model">The updated role model.</param>
        /// <param name="roleId">The ID of the role to be updated.</param>
        /// <returns>Returns Ok with a success message if the role is updated, NotFound if the role does not exist,
        /// or BadRequest with errors if the update fails.</returns>
        [HttpPut("{roleId}")]
        [ProducesResponseType(typeof(object), 200)] // Swagger response type for successful update
        [ProducesResponseType(typeof(object), 400)] // Swagger response type for bad request
        [ProducesResponseType(typeof(object), 404)] // Swagger response type for not found
        public async Task<IActionResult> UpdateRole([Required] RoleModel model, [Required, FromQuery] string roleId)
        {
            // Retrieve the role by ID
            var role = await _roleManager.FindByIdAsync(roleId);

            // Check if the role exists
            if (role == null)
            {
                // Return a 404 Not Found response with a custom message
                return NotFound(new
                {
                    Message = $"There is no role with ID = {roleId}."
                });
            }

            // Update the role properties
            role.Permissions = model.Permissions;
            role.Name = model.Name;

            // Attempt to update the role
            var result = await _roleManager.UpdateAsync(role);

            // Check if the update was successful
            if (result.Succeeded)
            {
                // Return a 200 OK response with a success message
                return Ok(new
                {
                    Message = $"Role with ID = {roleId} was updated successfully."
                });
            }

            // Return a BadRequest response with errors if the update failed
            return BadRequest(new
            {
                Message = $"Failed to update role with ID = {roleId}.",
                result.Errors
            });
        }


    }
}
