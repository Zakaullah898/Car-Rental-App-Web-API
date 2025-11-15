using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");
            builder.HasKey(l => l.LocationId).HasName("location_id");
            builder.Property(l => l.LocationId).HasColumnName("location_id").UseIdentityColumn();
            builder.Property(l => l.Address).HasMaxLength(100).HasColumnName("address").IsRequired();
            builder.Property(l => l.City).HasMaxLength(50).HasColumnName("city").IsRequired();

        }
    }
}
