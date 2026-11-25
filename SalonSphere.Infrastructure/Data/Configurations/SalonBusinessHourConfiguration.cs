

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class SalonBusinessHourConfiguration
    {
        public void Configure(EntityTypeBuilder<SalonBusinessHour> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Day).IsRequired().HasMaxLength(15);

            builder.HasIndex(x => new { x.SalonId, x.Day }).IsUnique();

            builder.HasOne(x => x.Salon).WithMany(x => x.BusinessHours).HasForeignKey(x => x.SalonId).OnDelete(DeleteBehavior.Cascade);
        }            
    }
}
