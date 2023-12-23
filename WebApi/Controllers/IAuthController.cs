using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers
{
    public interface IAuthController
    {
        Task<IActionResult> Register(RegisterModel model);
        Task<IActionResult> Login(LoginModel model);
        Task<IActionResult> Logout();
        Task<IActionResult> RefreshToken();
        Task<IActionResult> ForgotPassword();
        Task<IActionResult> ResetPassword();
        Task<IActionResult> ChangePassword();
        IActionResult GetUserInfo();
        Task<IActionResult> UpdateUserInfo();
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
