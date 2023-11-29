using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PentrationTestRepository : BaseRepository<PentrationTest>
    {
        public PentrationTestRepository(AppDbContext context) : base(context) { }
        public List<PentrationTest> GetAllRequestedTests()
        {
            return _context.Tests.Where(t => t.Status == TestStatus.OnHold).ToList();
        }
    }
}

