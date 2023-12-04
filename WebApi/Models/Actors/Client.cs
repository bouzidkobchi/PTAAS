using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Client : ApplicationUser
    {
        public Client() { }
        public string? Company { get; set; }
        public ICollection<PentrationTest> RequestedTests { get; set; } = new List<PentrationTest>();
        // payment card
        // 
    }
}
