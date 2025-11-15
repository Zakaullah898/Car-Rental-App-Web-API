namespace CarRentalApp.models
{
    public class Enums
    {
        public enum CarStatus
        {
            Available,
            Booked,
            Maintenance
        }
        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        public enum PaymentMethod
        {
            CreditCard,
            DebitCard,
            PayPal,
            Cash
        }
    }
}
