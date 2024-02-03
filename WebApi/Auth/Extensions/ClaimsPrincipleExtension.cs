using System.Security.Claims;
using WebApi.Auth.Helpers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Auth.Extenions
{
    public static class ClaimsPrincipleExtension
    {
        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user?.Identity?.IsAuthenticated == true;
        }

        public static string[] GetRoles(this ClaimsPrincipal User)
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            return roles;
        }

        public static UserInfo? GetUserInfo(this ClaimsPrincipal user)
        {
            if (!user.IsAuthenticated() || user.Identity is not ClaimsIdentity claimsIdentity)
            {
                // User is not authenticated
                return null;
            }

            return new UserInfo
            {
                Username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value,
                Roles = claimsIdentity.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToArray()
            };
        }

        public static ApplicationUser? LoadUser(this ClaimsPrincipal User , AppDbContext dbContext)
        {
            var uid = User.Claims.First(c => c.Type == "uid").Value;
            if (uid == null)
            {
                return null;
            }

            var user = dbContext.Users.Find(uid);

            return user;
        }

    }
}
