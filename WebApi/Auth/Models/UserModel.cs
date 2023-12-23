using WebApi.Models;

namespace WebApi.Auth.Models
{
    public class UserModel
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
