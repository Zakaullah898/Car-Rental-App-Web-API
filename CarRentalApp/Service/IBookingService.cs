using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public interface IBookingService
    {
        // Define method signatures related to booking operations here

        // method to create a new booking
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest model);
    }
}
