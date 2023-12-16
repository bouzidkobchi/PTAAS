using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebApi.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public object Errors { get; set; }
    }
}
