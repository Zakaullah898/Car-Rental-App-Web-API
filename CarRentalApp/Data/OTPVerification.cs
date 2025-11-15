using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.Data
{
    public class OTPVerification
    {
        public int OtpId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty; 
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsVerified { get; set; }

        public virtual User? User { get; set; }
    }
}
