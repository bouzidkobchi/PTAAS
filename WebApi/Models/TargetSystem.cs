using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    [PrimaryKey("Name")]
    public class TargetSystem
    {
        public required string Name { get; set; }
        public DbSet<PentrationTest>? Tests { get; set; }
    }
}
