using static CarRentalApp.models.Enums;

namespace CarRentalApp.Data
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Method { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
