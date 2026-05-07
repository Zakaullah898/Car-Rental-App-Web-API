namespace CarRentalApp.models
{
    public class CreditCardPaymentDto
    {
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? Expiry { get; set; }
        public string? CVC { get; set; }
    }
}
