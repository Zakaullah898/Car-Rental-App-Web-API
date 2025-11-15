using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.PaymentId).HasName("payment_id");
            builder.Property(p => p.PaymentId).HasColumnName("payment_id").UseIdentityColumn();
            builder.Property(p => p.BookingId).IsRequired().HasColumnName("booking_id");
            builder.Property(p => p.Amount).HasColumnName("amount").HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(p => p.PaymentDate).HasColumnName("payment_date").IsRequired();
            builder.Property(p => p.Method).HasMaxLength(20)
                   .HasColumnName("payment_method")
                   .IsRequired();

            // Foreign Key Relationship for Booking
            builder.HasOne(p => p.Booking)
                   .WithOne(b => b.Payment)
                   .HasForeignKey<Payment>(p => p.BookingId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("FK_Payments_Bookings_BookingId");
        }
    }
}
