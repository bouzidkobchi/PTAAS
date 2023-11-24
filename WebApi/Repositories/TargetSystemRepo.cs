using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class TargetSystemRepo : BaseRepository<TargetSystem>
    {
        public TargetSystemRepo(AppDbContext context) : base(context) { }
    }
}

