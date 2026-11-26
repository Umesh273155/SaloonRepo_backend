
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
    {
        public void Configure(EntityTypeBuilder<ServiceCategory> builder)
        {
            builder.ToTable("ServiceCategories");

            builder.HasKey(x => x.CategoryId);

            builder.Property(x => x.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(x => x.CategoryName)
                .IsUnique();

            // Default Service Categories
            builder.HasData(
                 new ServiceCategory
                 {
                     CategoryId = 1,
                     CategoryName = "Hair",
                     Description = "Hair cutting, styling and hair treatments",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 2,
                     CategoryName = "Skin Care",
                     Description = "Facial, cleanup and skincare treatments",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 3,
                     CategoryName = "Nail Care",
                     Description = "Nail extensions, nail art and nail treatments",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 4,
                     CategoryName = "Tattoo",
                     Description = "Tattoo designing and tattoo services",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 5,
                     CategoryName = "Makeup",
                     Description = "Party makeup, bridal makeup and professional makeup",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 6,
                     CategoryName = "Waxing",
                     Description = "Facial and body waxing services",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 7,
                     CategoryName = "Threading",
                     Description = "Eyebrow, upper lip and facial threading",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 },
                 new ServiceCategory
                 {
                     CategoryId = 8,
                     CategoryName = "Spa & Massage",
                     Description = "Hair spa, body massage and relaxation treatments",
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 11, 26)
                 }
             );
        }
    }
}
