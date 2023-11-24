using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class TargetSystemRepository : BaseRepository<TargetSystem>
    {
        public TargetSystemRepository(AppDbContext context) : base(context) { }
    }
}

