using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.models
{
    public class User : IdentityUser
    {
        public bool HasProfile { get; set; }
        public virtual ICollection<OTPVerification>? OTPVerifications { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual UserProfile? UserProfile { get; set; }
        public virtual ICollection<FavoriteCars>? FavoriteCars { get; set; } 
        public virtual ICollection<UserRefreshToken>? UserRefreshToken { get; set; }
    }
}
