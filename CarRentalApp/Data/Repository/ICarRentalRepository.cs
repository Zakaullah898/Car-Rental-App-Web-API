using System.Linq.Expressions;

namespace CarRentalApp.Data.Repository
{
    public interface ICarRentalRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> Filter, bool UseNoTracking =false);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(Expression<Func<T, bool>> Filter);
    }
}
