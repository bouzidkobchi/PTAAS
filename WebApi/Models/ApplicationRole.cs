using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Enums;

namespace WebApi.Models
{
    public class ApplicationRole<TKey> : IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        public List<ApplicationUser> Users { get; set; }
        [ConcurrencyCheck]
        public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public List<Permission> Permissions { get; set; } = new List<Permission>();
        public ApplicationRole()
        {
        }
        public bool HasPermission(Permission permission)
        {
            return Permissions.Contains(permission);
        }
    }

    public class ApplicationRole : ApplicationRole<string>
    {

    }
}
