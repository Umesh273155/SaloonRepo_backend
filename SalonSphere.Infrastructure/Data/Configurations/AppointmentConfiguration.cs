using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonSphere.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalonSphere.Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.AppointmentId);

            builder.Property(a => a.CustomerName).IsRequired().HasMaxLength(150);

            builder.Property(a => a.CustomerPhone).IsRequired().HasMaxLength(15);

            builder.Property(a => a.Amount).HasPrecision(10, 2);

            builder.Property(a => a.Status).IsRequired().HasMaxLength(30).HasDefaultValue("Pending");

            builder.Property(a => a.Notes).HasMaxLength(500);

            builder.Property(s => s.CreatedAt).IsRequired().ValueGeneratedNever();

            builder.HasOne(a => a.Salon).WithMany().HasForeignKey(a => a.SalonId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Service).WithMany().HasForeignKey(a => a.ServiceId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Staff).WithMany().HasForeignKey(a => a.StaffId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(a => new
            {
                a.SalonId,
                a.AppointmentDate
            });

            builder.HasIndex(a => new
            {
                a.StaffId,
                a.AppointmentDate,
                a.AppointmentTime
            });

        }
    }
}
