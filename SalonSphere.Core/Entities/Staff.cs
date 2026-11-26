
namespace SalonSphere.Core.Entities
{
    public class Staff
    {
        public int StaffId { get; set; }
        public int SalonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Salon Salon { get; set; } = null!;
    }
}
