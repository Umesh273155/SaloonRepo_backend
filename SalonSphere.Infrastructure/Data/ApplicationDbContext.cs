
using Microsoft.EntityFrameworkCore;
using SalonSphere.Core.Entities;
using User = SalonSphere.Core.Entities.User;


namespace SalonSphere.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Salon> Salons => Set<Salon>();

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<EmailOtp> EmailOtps => Set<EmailOtp>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}