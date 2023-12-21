using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Auth.Extenions;
using WebApi.Data;
using WebApi.Enums;

namespace WebApi.Auth.Helpers
{
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
        }
    }
}
