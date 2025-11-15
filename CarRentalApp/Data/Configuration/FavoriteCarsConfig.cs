using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class FavoriteCarsConfig : IEntityTypeConfiguration<FavoriteCars>
    {
        public void Configure(EntityTypeBuilder<FavoriteCars> builder)
        {
            builder.ToTable("FavoriteCars");
            builder.HasKey(fc => fc.FavoriteId).HasName("PK_FavoriteId");
            builder.Property(fc => fc.FavoriteId)
                .HasColumnName("favorite_id")
                .UseIdentityColumn();
            builder.Property(u => u.UserId)
                .HasColumnName("user_id")
                .IsRequired();
            builder.Property(c => c.CarId).HasColumnName("car_id").IsRequired();
            builder.Property(a => a.AddedAt)
                .HasColumnName("added_at")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();
            // Foreign Key Relationship with User and Car
            builder.HasOne(fc => fc.User)
                .WithMany(u => u.FavoriteCars)
                .HasForeignKey(fc => fc.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_FavoriteCars_Users_UserId");
            builder.HasOne(fc => fc.Car).WithMany(c => c.FavoriteCars)
                .HasForeignKey(fc => fc.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_FavoriteCars_Cars_CarId");
        }
    }
}
