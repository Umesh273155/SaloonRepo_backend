
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

            builder.Property(x => x.ProfileImage)
               .HasMaxLength(500);

            builder.Property(x => x.Salary)
               .HasPrecision(10, 2);

            builder.Property(s => s.IsActive).HasDefaultValue(true);

            builder.Property(s => s.CreatedAt) 
              .IsRequired()
              .ValueGeneratedNever();

            builder.HasOne(x => x.Salon)
              .WithMany()
              .HasForeignKey(x => x.SalonId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Designation)
                .WithMany(x => x.StaffMembers)
                .HasForeignKey(x => x.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.SalonId, x.Email });

            builder.HasIndex(x => new { x.SalonId, x.Phone });
        }
    }
}
