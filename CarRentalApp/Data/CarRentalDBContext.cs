using CarRentalApp.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Data
{
    public class CarRentalDBContext : IdentityDbContext
    {
        public CarRentalDBContext()
        { }
        public CarRentalDBContext(DbContextOptions<CarRentalDBContext> options)
            : base(options)
        {}

        public virtual DbSet<OTPVerification> OTPVerifications { get; set; }
        public virtual DbSet<Car> Cars { get; set;}
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<FavoriteCars> FavoriteCars { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CarConfig());
            modelBuilder.ApplyConfiguration(new BookingConfig());
            modelBuilder.ApplyConfiguration(new LocationConfig());
            modelBuilder.ApplyConfiguration(new PaymentConfig());
            modelBuilder.ApplyConfiguration(new UserProfileConfig());
            modelBuilder.ApplyConfiguration(new OtpVerificationConfig());
            modelBuilder.ApplyConfiguration(new FavoriteCarsConfig());
        }
        }
}
