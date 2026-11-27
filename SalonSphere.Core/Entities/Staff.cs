
namespace SalonSphere.Core.Entities
{
    public class Staff
    {
        public int StaffId { get; set; }
        public int SalonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }
        public int DesignationId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Salon Salon { get; set; } = null!;
        public Designation Designation { get; set; } = null!;
    }
}
