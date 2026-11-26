
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.ServiceId);
      

            builder.Property(x => x.ServiceId)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ServiceName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Price)
                .HasPrecision(10, 2);

            builder.Property(s => s.DurationMinutes)
                .IsRequired();

            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedNever();

            builder.HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Salon)
                .WithMany()
                .HasForeignKey(s => s.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new
            {
                s.SalonId,
                s.IsActive
            });
        }
    }
}
