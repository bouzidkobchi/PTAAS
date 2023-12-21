using System.ComponentModel.DataAnnotations;
using WebApi.Repositories;

namespace WebApi.Models
{
    public class Report : IHasId
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        public required string FilePath { get; set; }
        // created by attribute
    }
}
