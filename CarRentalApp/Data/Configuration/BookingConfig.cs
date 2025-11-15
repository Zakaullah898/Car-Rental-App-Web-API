using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class BookingConfig:IEntityTypeConfiguration<Booking>
    {
        public BookingConfig() { }
        public  void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(b => b.BookingId).HasName("booking_id");
            builder.Property(b => b.BookingId).HasColumnName("booking_id").UseIdentityColumn();
            builder.Property(b => b.CarId).IsRequired().HasColumnName("car_id");
            builder.Property(b => b.LocationId).IsRequired().HasColumnName("pickup_location_id");
            builder.Property(b => b.LocationId).IsRequired().HasColumnName("dropoff_location_id");
            builder.Property(b => b.StartDate).HasColumnName("pickup_date").IsRequired();
            builder.Property(b => b.EndDate).HasColumnName("return_date").IsRequired();
            builder.Property(b => b.TotalPrice).HasColumnName("total_amount").HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(b => b.Status).HasMaxLength(20).HasColumnName("status").IsRequired();
            // Forgein Key Relationships for car
            builder.HasOne(b => b.Car)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CarId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Bookings_Car_CarId");
            // Foreign Key Relationships for pickup and dropoff locations
            builder.HasOne(b => b.PickupLocation)
                .WithMany(l => l.PickupBookings)
                .HasForeignKey(b => b.LocationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Bookings_Location_PickupLocationId");

            builder.HasOne(b => b.DropoffLocation)
                .WithMany(l => l.DropOffBookings)
                .HasForeignKey(b => b.LocationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Bookings_Location_DropoffLocationId");
            // Foreign Key Relationships for User
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Bookings_Users_UserId");

        }
    }
}
