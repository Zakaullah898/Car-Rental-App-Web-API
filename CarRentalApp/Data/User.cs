using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.Data
{
    public class User : IdentityUser
    {
        public bool HasProfile { get; set; }
        public virtual ICollection<OTPVerification>? OTPVerifications { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual UserProfile? UserProfile { get; set; }
        public virtual ICollection<FavoriteCars>? FavoriteCars { get; set; } 
    }
}
