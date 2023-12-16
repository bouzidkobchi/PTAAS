using System.Security.Claims;
namespace WebApi.Helpers
{

    public static class JwtHelper
    {
        public static UserInfo GetUserInfo(HttpContext httpContext)
        {
            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                // User is not authenticated
                return null;
            }

            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                // Unable to retrieve claims
                return null;
            }

            var username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            var roles = claimsIdentity.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToList();

            return new UserInfo
            {
                Username = username,
                Email = email,
                Roles = roles
            };
        }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
