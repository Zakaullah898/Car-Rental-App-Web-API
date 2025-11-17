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

        public async Task<CarDTO> GetCarByIdAsync(int carId)
        {
            if(carId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), "CarId must be greater than zero.");
            }
            var car = await _carRepository.GetAsync(c => c.CarId == carId, false);
            if(car == null)
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }
            CarDTO carDTO = _mapper.Map<CarDTO>(car);
            return carDTO;
        }
    }
}
