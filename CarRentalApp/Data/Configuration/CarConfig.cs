using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class CarConfig : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");

            builder.HasKey(c => c.CarId).HasName("PK_Cars");

            builder.Property(c => c.CarId)
                .HasColumnName("car_id")
                .UseIdentityColumn();

            builder.Property(c => c.Title)
                .HasColumnName("title")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Category)
                .HasColumnName("category")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.ImageUrl)
                .HasColumnName("imageUrl")
                .HasMaxLength(255);

            builder.Property(c => c.TankCapacity)
                .HasColumnName("tank_capacity")
                .IsRequired();

            builder.Property(c => c.Transmission)
                .HasColumnName("transmission")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.SittingCapacity)
                .HasColumnName("sitting_capacity")
                .IsRequired();

            builder.Property(c => c.PricePerDay)
                .HasColumnName("price_per_day")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(c => c.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .HasDefaultValue("PKR");


            builder.Property(c => c.Status)
                .HasColumnName("status")
                .HasMaxLength(20);


        }
    }
}
