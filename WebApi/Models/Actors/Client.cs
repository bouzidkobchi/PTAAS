using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Client : ApplicationUser
    {
        [Length(2,150)]
        public string? Company { get; set; }
        public DbSet<PentrationTest>? RequestedTests { get; set; }
        // card
        // 
    }
}
