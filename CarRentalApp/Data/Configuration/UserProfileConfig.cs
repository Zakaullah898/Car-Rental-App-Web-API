using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");
            builder.HasKey(up => up.ProfileId);
            builder.Property(up => up.ProfileId).HasColumnName("profile_id").UseIdentityColumn();
            builder.Property(up => up.UserId).HasColumnName("user_id");
            builder.Property(up => up.FullName).HasMaxLength(100).HasColumnName("full_name");
            builder.Property(up => up.Email).IsRequired().HasMaxLength(100).HasColumnName("email");
            builder.Property(up => up.Phone).HasMaxLength(15).HasColumnName("phone");
            builder.Property(up => up.Gender).HasMaxLength(10).HasColumnName("gender");
            builder.Property(up => up.PostCode).HasMaxLength(20).HasColumnName("post_code");
            builder.Property(up => up.Address).HasMaxLength(200).HasColumnName("address");
            builder.Property(up => up.ProfileImageUrl).HasMaxLength(2048).HasColumnName("profile_image_url");
            builder.Property(up => up.DrivingLicenseNo).HasMaxLength(50).HasColumnName("driving_license_no");
            builder.Property(up => up.LicenseFrontImage).HasMaxLength(2048).HasColumnName("license_front_image");
            builder.Property(up => up.LicenseBackImage).HasMaxLength(2048).HasColumnName("license_back_image");
            builder.Property(up => up.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(up => up.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()").IsRequired();
            // Foreign Key Relationship with User
            builder.HasOne(up => up.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserProfiles_Users_UserId");

        }
    }
}
