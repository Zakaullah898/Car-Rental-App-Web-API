namespace CarRentalApp.Data
{
    public class Location
    {
        public int LocationId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        public virtual ICollection<Booking>? PickupBookings { get; set; }
        public virtual ICollection<Booking>? DropOffBookings { get; set; }
    }
}
