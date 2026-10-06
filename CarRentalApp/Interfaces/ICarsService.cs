using CarRentalApp.DTOs;

namespace CarRentalApp.Interfaces
{
    public interface ICarsService
    {
        // Define method signatures for car-related operations
        Task<List<CarDTO>> GetAllCarsAsync();
        Task<CarDTO> GetCarByIdAsync(int carId);
    }
}
