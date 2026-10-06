using AutoMapper;
using CarRentalApp.Data.Repository;
using CarRentalApp.DTOs;
using CarRentalApp.Interfaces;
using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public class LocationService : ILocationService
    {
        private readonly ICarRentalRepository<Location> _locationRepository;
        private readonly IMapper _mapper;
        public LocationService(
            ICarRentalRepository<Location> locationRepository,
            IMapper mapper
            )
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        public async Task<List<LocationDTO>> GetAllLocationsAsync()
        {
            var locations = await _locationRepository.GetAllAsync();
            return _mapper.Map<List<LocationDTO>>( locations );
        }
    }
}
