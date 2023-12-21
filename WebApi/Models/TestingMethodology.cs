using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Repositories;

namespace WebApi.Models
{
    public class TestingMethodology : IHasId
    {
        [NotMapped]
        public string Id => Name;
        [Key]
        public required string Name { get; set; }
        public ICollection<PentrationTest> Tests { get; set; } = new List<PentrationTest>();
    }
}
