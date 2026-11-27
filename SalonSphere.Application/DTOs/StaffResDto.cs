
namespace SalonSphere.Application.DTOs
{
    public class StaffResDto
    {
        public int StaffId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }

        public int DesignationId { get; set; }

        public string DesignationName { get; set; } = string.Empty;

        public DateTime? JoiningDate { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
