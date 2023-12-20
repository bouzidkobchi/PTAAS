using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using WebApi.Data;

namespace WebApi.Auth.Helpers
{

    public static class JwtHelper
    {
        public static UserInfo? GetUserInfo(HttpContext httpContext)
        {
            if (httpContext == null || !httpContext.User.IsAuthenticated())
            {
                // User is not authenticated
                return null;
            }

            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

            if (claimsIdentity == null) { return null; }

            return new UserInfo
            {
                Username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value,
                Roles = claimsIdentity.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToArray()
            };
        }
    }

    public class UserInfo
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string[]? Roles { get; set; }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public HasPermissionAttribute(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.IsAuthenticated())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roles = new AppDbContext().LoadRoles(user.GetRoles());

            if(!roles.Any(r => r.Permissions.Contains(Permission)))
            {
                context.Result = new ForbidResult();
            }

            //if (user.IsAuthenticated())
            //{
            //    if (user.GetRoles().Any(role => role.HasPermission(Permission)))
            //    {
            //        return;
            //    }
            //    context.Result = new ForbidResult();
            //}
            //else
            //{
            //    context.Result = new UnauthorizedResult();
            //}
            //throw new NotImplementedException();


        }
    }

        //public DateTime IsToday { get; set; }

        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    // skip authorization if action is decorated with [AllowAnonymous] attribute
        //    var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        //    if (allowAnonymous)
        //        return;

        //    // authorization
        //    //var user = context.HttpContext.User;
        //    //if (!user.Identity.IsAuthenticated || !user.HasClaim(ClaimType, ClaimValue))
        //    //{
        //    //    // not logged in or claim not present
        //    //    context.Result = new UnauthorizedResult();
        //    //}
        //    if(DateTime.Now == IsToday)
        //    {
        //        context.Result = new UnauthorizedResult();
        //    }

        //}

        //Console.WriteLine(context.Result.ToJson());
        //if (!context.HttpContext.Request.Query.ContainsKey(key))
        //{
        //    var user = context.HttpContext.User;
        //    if (user.Identity is null || !user.Identity.IsAuthenticated)
        //    {
        //        context.Result = new UnauthorizedResult();
        //    }
        //    else
        //    {
        //        context.Result = new ForbidResult();
        //    }

        //}

    //public class Permission
    //{
    //    public static RolePermissions RolePermissions { get; set; }
    //    public static TestPermissions TestPermissions { get; set; }
    //}

    //public class RolePermissions
    //{
    //    public bool CanCreate { get; set;} 
    //    public bool CanRead { get; set;} 
    //    public bool CanUpdate { get; set;} 
    //    public bool CanDelete { get; set;}
    //}

    //public class TestPermissions
    //{
    //    public bool CanCreate { get; set; }
    //    public bool CanRead { get; set; }
    //    public bool CanUpdate { get; set; }
    //    public bool CanDelete { get; set; }
    //    public bool CanAssignToPentester { get; set; }
    //    public bool CanRemoveFromPentester { get; set; }
    //}


    //public class Role
    //{
    //    public List<Permission> Permissions { get; set; }
    //    public bool HasPermission(Permission permission)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public enum Permission : byte
    {
        CanCreateUser,
        CanAssignTest
    }
}
