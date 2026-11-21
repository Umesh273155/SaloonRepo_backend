
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class EmailOtpConfiguration : IEntityTypeConfiguration<EmailOtp>
    {
        public void Configure(EntityTypeBuilder<EmailOtp> builder)
        {
            builder.HasKey(x => x.EmailOtpId);

            builder.Property(x => x.OtpHash)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.Attempts)
                .HasDefaultValue(0);

            builder.Property(x => x.IsUsed)
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // User Relationship
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster OTP lookup
            builder.HasIndex(x => x.UserId);
        }
    }
}
