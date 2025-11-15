using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarRentalApp.Data.Repository
{
    public class FavouriteRespository : CarRentalRepository<FavoriteCars>, IFavouriteRespository
    {
        private readonly CarRentalDBContext _dbContext;
        public FavouriteRespository(CarRentalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<FavoriteCars>> GetAll(Expression<Func<FavoriteCars, bool>>? filter1 = null, bool useNoTracking = false)
        {
            if(useNoTracking)
            {
                if(filter1 != null)
                {
                    return await _dbContext.Set<FavoriteCars>().AsNoTracking().Include(c => c.Car).Where(filter1).ToListAsync();
                }
                else
                {
                    return await _dbContext.Set<FavoriteCars>().AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if(filter1 != null)
                {
                    return await _dbContext.Set<FavoriteCars>().Where(filter1).Include(c => c.Car).ToListAsync();
                }
                else
                {
                    return await _dbContext.Set<FavoriteCars>().ToListAsync();
                }
            }

        }

    }
}
