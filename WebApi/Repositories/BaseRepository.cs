using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class BaseRepository<T> where T : class,IHasId
    {
        protected readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public string Create(T item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item.Id;
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(string id)
        {
            var itemToDelete = this.Get(id);
            if (itemToDelete != null)
            {
                this.Delete(itemToDelete);
            }
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public virtual T? Get(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>()
                           .AsNoTracking()
                           .ToList();
        }

        public virtual IEnumerable<T> GetPage(int page, int pageSize)
        {
            return _context.Set<T>()
                           .AsNoTracking()
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();
        }
    }
}
