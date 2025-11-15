using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CarRentalApp.Data.Repository
{
    public class CarRentalRepository<T> : ICarRentalRepository<T> where T : class
    {
        private readonly CarRentalDBContext _dbContext;
        private  DbSet<T> _dbSet;
        public CarRentalRepository(CarRentalDBContext dbContext)
        {
                this._dbContext = dbContext;
                _dbSet = dbContext.Set<T>();
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(Expression<Func<T, bool>> Filter)
        {
            var entity = await _dbSet.Where(Filter).FirstOrDefaultAsync();
            if(entity ==null)
            {
                throw new  KeyNotFoundException("Entity not found");
            }
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> Filter, bool UseNoTracking = false)
        {
            if(UseNoTracking)
            {
                return _dbSet.AsNoTracking().Where(Filter).FirstOrDefaultAsync()!;
            }
            else
            {
                return _dbSet.Where(Filter).FirstOrDefaultAsync()!;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
