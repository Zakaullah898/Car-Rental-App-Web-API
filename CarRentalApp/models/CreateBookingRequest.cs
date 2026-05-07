using static CarRentalApp.models.Enums;

namespace CarRentalApp.models
{
    public class CreateBookingRequest
    {
        public BillingInfoDto? Billing { get; set; }
        public RentalInfoDto? Rental { get; set; }
        public PaymentInfoDto? Payment { get; set; }

        public bool AcceptTerms { get; set; }
    }
}
