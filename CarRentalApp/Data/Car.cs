using static CarRentalApp.models.Enums;

namespace CarRentalApp.Data
{
    public class Car
    {
        public int CarId { get; set; }

        public string? Title { get; set; }        // e.g., Wagon-R
        public string? Category { get; set; }    // e.g., Sport, SUV, Sedan
        public string? ImageUrl { get; set; }

        public int TankCapacity { get; set; }    // e.g., 54 liters
        public int Transmission { get; set; } 
        public int SittingCapacity { get; set; } // e.g., 2, 4, 5

        public decimal PricePerDay { get; set; } // e.g., 5000.00
        public string? Currency { get; set; } = "PKR";

        public CarStatus Status { get; set; } // Available, Rented, Maintenance

        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<FavoriteCars>? FavoriteCars { get; set; }
    }

}
