using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class BaseRepository<TClass , Tkey> where TClass : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public Tkey? Create(TClass item, Func<TClass,Tkey> ReturnIdDelegate)
        {
            if (item is not null)
                _context.Add(item);
            else
                return default;

            _context.SaveChanges();
            return ReturnIdDelegate(item);
        }
    }
}
