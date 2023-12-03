using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repositories
{
    public class PentrationTestRepository : BaseRepository<PentrationTest>
    {
        public PentrationTestRepository(AppDbContext context) : base(context) { }

        public List<Finding>? Findings(string testId)
        {
            var selected_finding = _context.Tests
                                    .AsNoTracking()
                                    .Include(t => t.Findings)
                                    .FirstOrDefault(t => t.Id == testId);

            if (selected_finding == null)
                return null;

            return selected_finding.Findings.ToList();
        }

        public List<PentrationTest> SelectStatus(TestStatus status, int pageNumber = 1, int pageSize = 10)
        {
            return _context.Tests
                           .AsNoTracking()
                           .Where(t => t.Status == status)
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();
        }

    }
}

