using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Enums;
using WebApi.Repositories;

namespace WebApi.Models
{
    public class PentrationTest : IHasId
    {
        public string Id { get; set; }
        public  TargetSystem System { get; set; }
        public required string SystemId { get; set; }
        public ICollection<PentestingMethodology> Methodologies { get; set; } = new List<PentestingMethodology>();
        public TestStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ScheduleTime { get; set; }
        public  Client Owner { get; set; }
        public required string OwnerId { get; set; }
        public ICollection<Pentester> Pentesters { get; set; } = new List<Pentester>();
        public ICollection<Finding> Findings { get; set; } = new List<Finding>();
        // scope :
        // ip ranges ,
        // report file path

        // the scope :
        //public string IpRangeStart { get; set; }
        //public string IpRangeEnd { get; set; }
        //public string[] Protocols { get; set; }
        //[Length(20,500)]
        //public string TestObjective { get; set; }
    }

}
