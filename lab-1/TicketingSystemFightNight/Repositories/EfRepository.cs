using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;

namespace TicketingSystemFightNight.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly VjezbaDbContext _context;

        public EfRepository(VjezbaDbContext context)
        {
            _context = context;
        }

        public Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            var set = query != null ? query(_context.Set<T>()) : _context.Set<T>();
            return set.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            var set = query != null ? query(_context.Set<T>()) : _context.Set<T>();
            return await set.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id);
        }

        public Task AddAsync(T entity)
        {
            return _context.Set<T>().AddAsync(entity).AsTask();
        }

        public Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
