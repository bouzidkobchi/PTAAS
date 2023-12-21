using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;
using WebApi.Auth.Helpers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Auth.Extenions
{
    public static class ClaimsPrincipleExtension
    {
        //public static bool IsAuthenticated(this ClaimsPrincipal User)
        //{
        //    if (User is not null && User.Identity is not null && User.Identity.IsAuthenticated)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

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
            if (!user.IsAuthenticated())
            {
                // User is not authenticated
                return null;
            }

            var claimsIdentity = user.Identity as ClaimsIdentity;

            if (claimsIdentity == null) { return null; }

            return new UserInfo
            {
                Username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value,
                Roles = claimsIdentity.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToArray()
            };
        }

    }
}
