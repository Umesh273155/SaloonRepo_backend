
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staff");

            builder.HasKey(s => s.StaffId);

            builder.Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Phone)
                .HasMaxLength(15);

            builder.Property(s => s.Email)
                .HasMaxLength(150);

            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedAt)
          .IsRequired()
          .ValueGeneratedNever();

            builder.HasOne(s => s.Salon)
                .WithMany()
                .HasForeignKey(s => s.SalonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
