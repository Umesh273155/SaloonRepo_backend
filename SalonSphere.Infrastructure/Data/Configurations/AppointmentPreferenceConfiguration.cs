

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class AppointmentPreferenceConfiguration : IEntityTypeConfiguration<AppointmentPreference>
    {
        public void Configure(EntityTypeBuilder<AppointmentPreference> builder)
        {
            builder.HasKey(x => x.AppointmentPreferenceId);

            builder.Property(x => x.TimeZone)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.SalonId)
                .IsUnique();

            builder.HasOne(x => x.Salon)
                .WithOne(x => x.AppointmentPreference)
                .HasForeignKey<AppointmentPreference>(x => x.SalonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
