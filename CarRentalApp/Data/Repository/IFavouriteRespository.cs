using System.Linq.Expressions;

namespace CarRentalApp.Data.Repository
{
    public interface IFavouriteRespository:ICarRentalRepository<FavoriteCars>
    {
        Task<List<FavoriteCars>> GetAll(Expression<Func<FavoriteCars, bool>>? filter = null, bool useNoTracking = false);

    }
}
