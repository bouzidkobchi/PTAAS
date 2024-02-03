using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Auth.Models;

namespace WebApi.Controllers
{
    public interface IRoleController
    {
        Task<IActionResult> CreateRole([Required] RoleModel model); /*NewRoleModel*/
        Task<IActionResult> UpdateRole([Required] RoleModel model ,[Required] string roleId); /*UpdateRoleModel*/
        Task<IActionResult> DeleteRole([Required] string id);
        IActionResult GetRoles();
        Task<IActionResult> GetRoleById([Required] string id);

    }
}
