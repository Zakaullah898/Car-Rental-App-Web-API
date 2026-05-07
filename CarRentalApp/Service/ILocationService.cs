using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public interface ILocationService
    {
        // Define method signatures for location-related operations

        // interface for getting all locations
        Task<List<LocationDTO>> GetAllLocationsAsync();
    }
}
