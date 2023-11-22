using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Enums;

namespace WebApi.Models
{
    public class PentrationTest
    {
        public int Id { get; set; }
        public required TargetSystem System { get; set; }
        public required string SystemId { get; set; }
        public ICollection<PentestingMethodology> Methodologies { get; set; } = new List<PentestingMethodology>();
        public TestStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ScheduleTime { get; set; }
        public required Client Owner { get; set; }
        public required string OwnerId { get; set; }
        public ICollection<Pentester> Pentesters { get; set; } = new List<Pentester>();
        // target system x
        // scope :
        // ip ranges , 
        // methodologies x
        // status x
        // scheduletime x
        // creationtime x
        // report / reports
    }
}
