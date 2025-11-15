using static CarRentalApp.models.Enums;

namespace CarRentalApp.Data
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
