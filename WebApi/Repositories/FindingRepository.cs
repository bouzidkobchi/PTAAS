using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class FindingRepository : BaseRepository<Finding>
    {
        public FindingRepository(AppDbContext context) : base(context) { }
    }

}
