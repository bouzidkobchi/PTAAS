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
        Task<IActionResult> GetUserInfo();
        Task<IActionResult> UpdateUserInfo();

    }

    public interface IRoleController
    {
        /*
         * create , read , update [permmisions ..] , delete
         * assign role / remove a role from user
         * 
         */
    }

    public interface IUsersController
    {
        /*
         * crud
         * add role
         * 
         */
    }

    public interface ITestController
    {
        /*
         * assign it to a user [admin or some one else]
         * demand one [client]
         */
    }
}
