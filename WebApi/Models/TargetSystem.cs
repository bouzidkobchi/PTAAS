using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    [PrimaryKey("Name")]
    public class TargetSystem
    {
        public required string Name { get; set; }
        public ICollection<PentrationTest> Tests { get; set; } = new List<PentrationTest>();
    }
}
