using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Repositories;

namespace WebApi.Models
{
    [PrimaryKey("Name")]
    public class TargetSystem : IHasId
    {
        [NotMapped]
        public string Id => Name;
        public required string Name { get; set; }
        public ICollection<PentrationTest> Tests { get; set; } = new List<PentrationTest>();
    }
}
