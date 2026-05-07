namespace CarRentalApp.models
{
    public class RentalInfoDto
    {
        public int CarId { get; set; }

        public int PickupLocationId { get; set; }
        public DateTime PickupDate { get; set; }
        public string? PickupTime { get; set; }

        public int DropoffLocationId { get; set; }
        public DateTime DropoffDate { get; set; }
        public string? DropoffTime { get; set; }
    }
}
