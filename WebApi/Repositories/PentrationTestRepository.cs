using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PentrationTestRepository : BaseRepository<PentrationTest>
    {
        public PentrationTestRepository(AppDbContext context) : base(context) { }
    }
}
