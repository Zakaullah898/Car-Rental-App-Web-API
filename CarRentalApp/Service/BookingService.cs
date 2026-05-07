using AutoMapper;
using CarRentalApp.Data;
using CarRentalApp.Data.Repository;
using CarRentalApp.models;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Errors.Model;
using static CarRentalApp.models.Enums;

namespace CarRentalApp.Service
{
    public class BookingService : IBookingService
    {
        private readonly ICarRentalRepository<Booking> _bookiRepository;
        private readonly ICarRentalRepository<Payment> _paymentRepository;
        private readonly ICarRentalRepository<Location> _locationRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public BookingService(
            ICarRentalRepository<Booking> bookiRepository,
            ICarRentalRepository<Payment> paymentRepository,
            ICarRentalRepository<Location> locationRepository,
            IMapper mapper,
            UserManager<User> userManager
            )
        {
            _bookiRepository = bookiRepository;
            _paymentRepository = paymentRepository;
            _locationRepository = locationRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest dto)
        {
            if (!dto.AcceptTerms)
                throw new  BadRequestException("You must accept the terms and privacy policy.");
            var pickupExists = await _locationRepository.GetAsync(loc => loc.LocationId == dto.Rental!.PickupLocationId);
            var dropoffExists = await _locationRepository.GetAsync(loc => loc.LocationId == dto.Rental!.DropoffLocationId);

            if (pickupExists == null || dropoffExists ==null)
                throw new BadRequestException("Invalid pickup or dropoff location. OR Not found pickup or dropoff location");
            // validating user is exist in the system
            var user = await _userManager.FindByIdAsync(dto.Billing!.UserId!);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // Calculate price
            int totalDays = (dto.Rental!.DropoffDate - dto.Rental.PickupDate).Days;
            decimal totalPrice = totalDays * 1500;

            // MAP BOOKING USING AUTOMAPPER
            var booking = _mapper.Map<Booking>(dto);
            booking.UserId = user.Id;
            booking.Status = BookingStatus.Pending;
            booking.TotalPrice = totalPrice;

             await _bookiRepository.CreateAsync(booking);

            // Create payment entry
            var payment = _mapper.Map<Payment>(dto.Payment);
            payment.BookingId = booking.BookingId;
            payment.Amount = totalPrice;
            payment.PaymentStatus = "Pending";
            payment.TransactionId = Guid.NewGuid().ToString();

            await _paymentRepository.CreateAsync(payment);

            // Prepare response
            var response = new BookingResponse
            {
                BookingId = booking.BookingId,
                PaymentId = payment.PaymentId,
                Status = booking.Status.ToString(),
                TotalAmount = booking.TotalPrice,
                BookingDate = booking.StartDate
            };

            return response;
        }
    }
}
