using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Security.Claims;
using WebApi.Auth.Helpers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Auth
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



    }
}
