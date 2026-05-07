using System.Linq.Expressions;

namespace TicketingSystemFightNight.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? query = null);
        Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? query = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
