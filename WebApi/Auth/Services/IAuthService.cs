using WebApi.Models;

namespace WebApi.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(LoginModel model);
        Task<string> AddRoleAsync(RegisterRoleModel model);
    }
}
