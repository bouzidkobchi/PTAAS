using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Auth.Models;

namespace WebApi.Controllers
{
    public interface IUsersController
    {
        Task<IActionResult> CreateUserAsync([FromBody] UserModel model); // for admin only
        IActionResult GetUsersPage(int pageNumber, int pageSize);
        IActionResult GetUser(string id);
        Task<IActionResult> AddToRoleAsync(string id, string roleName);
        Task<IActionResult> RemoveFromRoleAsync(string id, string roleName);
        Task<IActionResult> UpdateUserInfoAsync([Required] UserModel model, [Required, FromRoute] string userId);
    }
}
