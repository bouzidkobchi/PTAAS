using WebApi.Enums;

namespace WebApi.Auth.Models
{
    public class RoleModel
    {
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
