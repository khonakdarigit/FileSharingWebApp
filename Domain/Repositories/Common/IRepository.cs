using System.Linq.Expressions;


namespace Domain.Repositories.Common
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> FindAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task SaveChangesAsync();
    }
}
