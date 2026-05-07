namespace CarRentalApp.models
{
    public class BookingResponse
    {
        public int BookingId { get; set; }
        public int PaymentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
