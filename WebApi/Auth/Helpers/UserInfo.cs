using System.Security.Claims;

namespace WebApi.Auth.Helpers
{
    public class UserInfo
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string[]? Roles { get; set; }
    }
}
