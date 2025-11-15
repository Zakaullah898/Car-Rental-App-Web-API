namespace CarRentalApp.models
{
    public class OtpVerificationDTO
    {
        public int OtpId { get; set; }
        public string UserId { get; set; } = string.Empty;  
        public string OtpCode { get; set; } = string.Empty; 
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
