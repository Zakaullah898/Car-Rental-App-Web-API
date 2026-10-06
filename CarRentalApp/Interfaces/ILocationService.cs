using CarRentalApp.DTOs;

namespace CarRentalApp.Interfaces
{
    public interface ILocationService
    {
        // Define method signatures for location-related operations

        // interface for getting all locations
        Task<List<LocationDTO>> GetAllLocationsAsync();
    }
}
