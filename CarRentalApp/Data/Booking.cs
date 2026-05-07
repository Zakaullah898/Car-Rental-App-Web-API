using static CarRentalApp.models.Enums;

namespace CarRentalApp.Data
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public string? UserId { get; set; }

        // FIX: Two separate location foreign keys
        public int PickupLocationId { get; set; }
        public int DropoffLocationId { get; set; }

        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string? Address { get; set; }

        public virtual Car Car { get; set; } = default!;
        public virtual Location PickupLocation { get; set; } = default!;
        public virtual Location DropoffLocation { get; set; } = default!;
        public virtual Payment? Payment { get; set; }
        public virtual User User { get; set; } = default!;
    }

}
