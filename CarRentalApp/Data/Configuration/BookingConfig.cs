using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class BookingConfig:IEntityTypeConfiguration<Booking>
    {
        public BookingConfig() { }
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.BookingId).HasName("PK_Bookings");

            builder.Property(b => b.BookingId)
                .HasColumnName("booking_id")
                .UseIdentityColumn();

            builder.Property(b => b.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(b => b.CarId)
                .IsRequired()
                .HasColumnName("car_id");

            builder.Property(b => b.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");

            builder.Property(b => b.City)
                .HasMaxLength(100)
                .HasColumnName("city");

            builder.Property(b => b.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");

            builder.Property(b => b.PickupLocationId)
                .IsRequired()
                .HasColumnName("pickup_location_id");

            builder.Property(b => b.DropoffLocationId)
                .IsRequired()
                .HasColumnName("dropoff_location_id");

            builder.Property(b => b.StartDate)
                .HasColumnName("pickup_date")
                .IsRequired();

            builder.Property(b => b.EndDate)
                .HasColumnName("return_date")
                .IsRequired();

            builder.Property(b => b.TotalPrice)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(b => b.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(b => b.Address)
                .HasMaxLength(200)
                .HasColumnName("address");

            // Car Relationship
            builder.HasOne(b => b.Car)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CarId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Bookings_Cars");

            // Pickup Location Relationship
            builder.HasOne(b => b.PickupLocation)
                .WithMany(l => l.PickupBookings)
                .HasForeignKey(b => b.PickupLocationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Bookings_PickupLocation");

            // Dropoff Location Relationship
            builder.HasOne(b => b.DropoffLocation)
                .WithMany(l => l.DropOffBookings)
                .HasForeignKey(b => b.DropoffLocationId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Bookings_DropoffLocation");

            // User Relationship
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Bookings_Users");
        }

    }
}
