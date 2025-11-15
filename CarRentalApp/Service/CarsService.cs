using AutoMapper;
using CarRentalApp.Data;
using CarRentalApp.Data.Repository;
using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public class CarsService : ICarsService
    {
        private readonly ICarRentalRepository<Car> _carRepository;
        private readonly IMapper _mapper;
        public CarsService(ICarRentalRepository<Car> carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
        public async Task<List<CarDTO>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllAsync();
            Console.WriteLine(cars);
            return _mapper.Map<List<CarDTO>>(cars);

        }
    }
}
