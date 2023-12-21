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
        public ICollection<TestingMethodology> Methodologies { get; set; } = new List<TestingMethodology>();
        public TestStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ScheduleTime { get; set; }
        public  ApplicationUser Owner { get; set; }
        public required string OwnerId { get; set; }
        public ICollection<ApplicationUser> Pentesters { get; set; } = new List<ApplicationUser>();
        public ICollection<Finding> Findings { get; set; } = new List<Finding>();
        // scope :q
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
