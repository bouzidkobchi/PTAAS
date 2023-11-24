using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PentrationTestRepo : BaseRepository<PentrationTest>
    {
        public PentrationTestRepo(AppDbContext context) : base(context) { }
    }
}
