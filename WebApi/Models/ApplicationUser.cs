using Microsoft.AspNetCore.Identity;
using WebApi.Repositories;

namespace WebApi.Models
{
    abstract public class ApplicationUser : IdentityUser , IHasId
    {
    }
}
