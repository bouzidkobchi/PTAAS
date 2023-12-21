using Microsoft.AspNetCore.Identity;
using WebApi.Repositories;

namespace WebApi.Models
{
    public class ApplicationUser : IdentityUser , IHasId
    {
        // canDoATest
        public ICollection<PentrationTest> TokenTests { get; set; } // make them as functions // TestsToDo

        // canDemandATest
        public string? Company { get; set; }
        public ICollection<PentrationTest> RequestedTests { get; set; } = new List<PentrationTest>(); // TestsToAsk

        // 
    }
}
