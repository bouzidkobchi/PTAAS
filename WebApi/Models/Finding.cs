using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Enums;
using WebApi.Repositories;

namespace WebApi.Models
{
    public class Finding : IHasId
    {
        public required string Id { get; set; }
        public string? Description { get; set; }
        public Severity Severity { get; set; }
        public required string FounderId { get; set; }
        public required ApplicationUser Founder { get; set; }
        public required string TestId { get; set; }
        public required PentrationTest Test { get; set; }
    }
}
