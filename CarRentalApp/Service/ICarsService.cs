using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public interface ICarsService
    {
        // Define method signatures for car-related operations
        Task<List<CarDTO>> GetAllCarsAsync();
    }
}
