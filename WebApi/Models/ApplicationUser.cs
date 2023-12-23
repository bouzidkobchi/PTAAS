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

        public override required string UserName { get => base.UserName; set => base.UserName = value; }
        public override required string Email { get => base.Email; set => base.Email = value; }
    }
}
