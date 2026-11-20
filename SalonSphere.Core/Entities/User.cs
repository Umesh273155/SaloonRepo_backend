
namespace SalonSphere.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public int? SalonId { get; set; }
        public int RoleId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Salon? Salon { get; set; }
        public Role Role { get; set; } = null!;
    }
}
