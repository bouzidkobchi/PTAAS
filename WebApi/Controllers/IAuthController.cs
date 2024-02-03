using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Auth;
using WebApi.Auth.Models;

namespace WebApi.Controllers
{
    public interface IAuthController
    {
        Task<IActionResult> Register(RegisterModel model);
        Task<IActionResult> Login(LoginModel model);
        Task<IActionResult> Logout();
        Task<IActionResult> RefreshToken(string refreshToken);
        Task<IActionResult> ForgotPassword(string email);
        Task<IActionResult> ResetPassword([Required, FromBody] ResetPasswordModel model);
        Task<IActionResult> ChangePassword(changePasswordModel model);
        IActionResult GetMyInfo();
    }

    public interface IAdminController
    {
    }

    public interface IPentesterController
    {
    }

    public interface IClientController
    {
    }
}
