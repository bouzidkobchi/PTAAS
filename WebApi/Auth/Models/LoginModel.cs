using System.ComponentModel.DataAnnotations;

namespace WebApi.Auth
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
