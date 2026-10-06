using CarRentalApp.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class UserRefreshTokenConfig : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("UserRefreshTokens");
            builder.HasKey(urt => urt.Id).HasName("PK_UserRefreshTokens");
            builder.Property(urt => urt.Id)
                .HasColumnName("id")
                .UseIdentityColumn();
            builder.Property(urt => urt.UserName)
                .HasColumnName("user_name")
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(urt => urt.RefreshToken)
                .HasColumnName("refresh_token")
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(urt => urt.ExpiryDate)
                .HasColumnName("expiry_date")
                .IsRequired();
            builder.Property(urt => urt.IsRevoked)
                .HasColumnName("is_revoked")
                .IsRequired();

            builder.HasOne(urt => urt.User)
                .WithMany(u => u.UserRefreshToken)
                .HasForeignKey(urt => urt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
