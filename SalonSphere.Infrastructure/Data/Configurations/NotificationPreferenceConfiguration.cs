
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{

    public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<SalonNotificationPreference>
    {
        public void Configure(EntityTypeBuilder<SalonNotificationPreference> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.SalonId).IsUnique();
            builder.HasOne<Salon>().WithMany().HasForeignKey(x => x.SalonId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
