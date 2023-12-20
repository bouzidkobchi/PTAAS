using Microsoft.AspNetCore.Authorization;
using WebApi.Auth.Helpers;
using WebApi.Data;

//namespace WebApi.Auth.Services
//{
//    // Define a custom requirement that checks for a permission
//    public class HasPermissionRequirement : IAuthorizationRequirement
//    {
//        public HasPermissionRequirement(Permission permission)
//        {
//            Permission = permission;
//        }

//        public Permission Permission { get; }
//    }

//    // Define a custom handler that handles the requirement
//    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
//    {
//        private readonly AppDbContext _dbContext;

//        // Use dependency injection to get the AppDbContext
//        public HasPermissionHandler(AppDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
//        {
//            try
//            {
//                // Get the user from the context
//                var user = context.User;

//                // Check if the user is authenticated
//                if (!user.Identity.IsAuthenticated)
//                {
//                    // Return unauthorized result
//                    context.Fail();
//                    return Task.CompletedTask;
//                }

//                // Get the roles of the user
//                var roles = _dbContext.LoadRoles(user.GetRoles());

//                // Check if any of the roles has the required permission
//                if (roles.Any(r => r.Permissions.Contains(requirement.Permission)))
//                {
//                    // Succeed the requirement
//                    context.Succeed(requirement);
//                }
//                else
//                {
//                    // Return forbidden result
//                    context.Fail();
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle any exceptions
//                // You can log the exception, return a custom result, or rethrow the exception
//                context.Fail();
//            }

//            return Task.CompletedTask;
//        }
//    }

//    // Define a custom attribute that uses the requirement and the handler
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    public class HasPermissionAttribute : AuthorizeAttribute
//    {
//        public HasPermissionAttribute(Permission permission) : base($"HasPermission{permission}")
//        {
//            Permission = permission;
//        }

//        public Permission Permission { get; }
//    }

//}
