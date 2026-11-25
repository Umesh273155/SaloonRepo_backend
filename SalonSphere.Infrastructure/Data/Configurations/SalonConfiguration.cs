

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class SalonConfiguration : IEntityTypeConfiguration<Salon>
    {
        public void Configure(EntityTypeBuilder<Salon> builder)
        {
            builder.HasKey(x => x.SalonId);
            builder.Property(x => x.SalonName).HasMaxLength(150).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();

            builder.Property(x => x.Email).HasMaxLength(150);

            builder.Property(x => x.Address).HasMaxLength(300);

            builder.Property(x => x.City).HasMaxLength(100);

            builder.Property(x => x.State).HasMaxLength(100);

            builder.Property(x => x.Pincode).HasMaxLength(10);
            builder.Property(x => x.LogoUrl).HasMaxLength(500);
        }
    }
}
