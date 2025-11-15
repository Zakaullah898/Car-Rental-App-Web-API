using AutoMapper;
using CarRentalApp.CustomException;
using CarRentalApp.Data;
using CarRentalApp.Data.Repository;
using CarRentalApp.models;
using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.Service
{
    public class FavouriteCarsService:IFavouriteCarsService
    {
        private readonly IFavouriteRespository _FavouriteCarsRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public FavouriteCarsService(
            IFavouriteRespository FavouriteCarsRepository,
            IMapper mapper,
            UserManager<User> userManager
            )
        {
            _FavouriteCarsRepository = FavouriteCarsRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        // Implement methods for managing favourite cars here
        public async Task<FavouriteDTO> AddToFavouritesAsync(FavouriteDTO entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "FavouriteCarsDTO cannot be null");
            }
            var existingFavourite = await _FavouriteCarsRepository
                .GetAsync(fav => fav.UserId == entity.UserId && fav.CarId == entity.CarId, true);
            if (existingFavourite != null)
            {
                // Already in favourites
                throw new  ConflictException("Car is already in favorites");
            }

            entity.AddedAt = DateTime.UtcNow;
            var favouriteCar = _mapper.Map<FavoriteCars>(entity);
            await _FavouriteCarsRepository.CreateAsync(favouriteCar);
            var resultDto = _mapper.Map<FavouriteDTO>(favouriteCar);
            return resultDto;
        }

        public async Task<List<FavouriteCarsDTO>> GetFavouriteCarsByUserIdAsync(string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null or empty");
            }
            var favouriteCars =await _FavouriteCarsRepository.GetAll(List => List.UserId == userId, true);
            if(favouriteCars == null || !favouriteCars.Any())
            {
                return new List<FavouriteCarsDTO>();
            }
            var FavouriteCars = favouriteCars.Select(fav => fav.Car).ToList();
            var favouriteCarsDTOs = _mapper.Map<List<FavouriteCarsDTO>>(FavouriteCars);

            return favouriteCarsDTOs;
        }

      
    }
}
