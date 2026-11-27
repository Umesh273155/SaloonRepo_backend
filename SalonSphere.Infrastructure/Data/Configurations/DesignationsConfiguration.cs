

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class DesignationsConfiguration : IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> builder)
        {
            builder.ToTable("Designations");

            builder.HasKey(x => x.DesignationId);

            builder.Property(x => x.DesignationName).HasMaxLength(100).IsRequired();

            builder.HasIndex(x => x.DesignationName).IsUnique();

            builder.HasData(
                new Designation
                {
                    DesignationId = 1,
                    DesignationName = "Manager",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 2,
                    DesignationName = "Senior Beautician",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 3,
                    DesignationName = "Beautician",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 4,
                    DesignationName = "Hair Stylist",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 5,
                    DesignationName = "Makeup Artist",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 6,
                    DesignationName = "Nail Artist",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 7,
                    DesignationName = "Receptionist",
                    IsActive = true
                },
                new Designation
                {
                    DesignationId = 8,
                    DesignationName = "Tattoo Artist",
                    IsActive = true
                }
            );
        }
    }
}
