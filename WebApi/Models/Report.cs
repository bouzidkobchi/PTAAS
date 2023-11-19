using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        public required string FilePath { get; set; }
    }
}
