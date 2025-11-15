using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalApp.Data.Configuration
{
    public class OtpVerificationConfig : IEntityTypeConfiguration<OTPVerification>
    {
        public void Configure(EntityTypeBuilder<OTPVerification> builder)
        {
            builder.ToTable("OTPVerifications");
            builder.HasKey(o => o.OtpId).HasName("otp_id");
            builder.Property(o => o.OtpId).HasColumnName("otp_id").UseIdentityColumn();
            builder.Property(o => o.UserId).HasMaxLength(450).HasColumnName("user_id").IsRequired();
            builder.Property(o => o.OtpCode).HasMaxLength(5).HasColumnName("otp_code").IsRequired();
            builder.Property(o => o.GeneratedAt).HasColumnName("generated_at").IsRequired();
            builder.Property(o => o.ExpiresAt).HasColumnName("expires_at").IsRequired();
            builder.Property(o => o.IsVerified).HasColumnName("is_verified").IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(u => u.OTPVerifications)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OTPVerifications_Users_UserId");
        }
    }
}
