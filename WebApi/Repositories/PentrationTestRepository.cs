using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repositories
{
    public class PentrationTestRepository : BaseRepository<PentrationTest>
    {
        public PentrationTestRepository(AppDbContext context) : base(context) { }

        //public List<Finding>? GetFindings(string testId, int pageNumber = 1, int pageSize = 10)
        //{
        //    var findings = _context.Findings
        //                           .AsNoTracking()
        //                           .Where(f => f.TestId == testId)
        //                           .Skip((pageNumber - 1) * pageSize)
        //                           .Take(pageSize)
        //                           .ToList();

        //    return findings.Count != 0 ? findings : null;
        //}

        //public List<Finding>? GetFindings(string testId, int pageNumber = 1, int pageSize = 10)
        //{
        //    // Check if the test exists
        //    var testExists = _context.Tests.Any(t => t.Id == testId);

        //    if (!testExists)
        //        return null;

        //    // If the test exists, query the findings
        //    var findings = _context.Findings
        //                           .AsNoTracking()
        //                           .Where(f => f.TestId == testId)
        //                           .Skip((pageNumber - 1) * pageSize)
        //                           .Take(pageSize)
        //                           .ToList();

        //    return findings;
        //}


        public List<Finding>? GetFindings(string testId, int pageNumber = 1, int pageSize = 10)
        {
            var test = _context.Tests
                               .AsNoTracking()
                               .Include(t => t.Findings)
                               .FirstOrDefault(t => t.Id == testId);

            if (test == null)
                return null;

            var findings = test.Findings
                               .Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToList();

            return findings;
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

