namespace CarRentalApp.models
{
    public class PaymentInfoDto
    {
        public string? PaymentMethod { get; set; } // CreditCard | PayPal | Bitcoin

        public CreditCardPaymentDto? CreditCard { get; set; }

    }
}
