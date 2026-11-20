

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.RoleId);
            builder.Property(x => x.RoleName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.HasIndex(x => x.RoleName).IsUnique();
            builder.HasData(
                  new Role { RoleId = 1, RoleName = "SuperAdmin" },
                  new Role { RoleId = 2, RoleName = "SalonOwner" },
                  new Role { RoleId = 3, RoleName = "Manager" },
                  new Role { RoleId = 4, RoleName = "Receptionist" },
                  new Role { RoleId = 5, RoleName = "Staff" }
              );
        }
    }
}
