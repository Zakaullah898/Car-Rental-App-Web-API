using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public interface IFavouriteCarsService
    {
        // Define methods for managing favourite cars here
        Task<FavouriteDTO> AddToFavouritesAsync(FavouriteDTO entity);
        // Adding method for getting favourite cars by user id and car id
        Task<List<FavouriteCarsDTO>> GetFavouriteCarsByUserIdAsync(string userId);

        // Method to remove a car from favourites
        Task<FavouriteDTO> RemoveFromFavouritesAsync(string userId, int carId);
    }
}
