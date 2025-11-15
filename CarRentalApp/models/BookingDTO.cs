using static CarRentalApp.models.Enums;

namespace CarRentalApp.models
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
    }
}
