using static CarRentalApp.models.Enums;

namespace CarRentalApp.Data
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public string? UserId { get; set; }
        public int LocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }

        public virtual required Car  Car { get; set; }
        public virtual required Location PickupLocation { get; set; }
        public virtual required Location DropoffLocation { get; set; }
        public virtual required Payment? Payment { get; set; }
        public virtual required User User { get; set; }
    }
}
