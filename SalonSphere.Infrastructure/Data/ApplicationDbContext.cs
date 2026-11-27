
using Microsoft.EntityFrameworkCore;
using SalonSphere.Core.Entities;
using User = SalonSphere.Core.Entities.User;


namespace SalonSphere.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Salon> Salons => Set<Salon>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<EmailOtp> EmailOtps => Set<EmailOtp>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
        public DbSet<SalonBusinessHour> SalonBusinessHours { get; set; }
        public DbSet<AppointmentPreference> AppointmentPreferences => Set<AppointmentPreference>();
        public DbSet<SalonNotificationPreference> SalonNotificationPreferences { get; set; }     
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
        public DbSet<Staff> StaffMembers => Set<Staff>();
        public DbSet<Designation> Designations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}