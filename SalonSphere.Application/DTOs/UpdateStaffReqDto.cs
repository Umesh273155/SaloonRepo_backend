

using System.ComponentModel.DataAnnotations;

namespace SalonSphere.Application.DTOs
{
    public class UpdateStaffReqDto
    {
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ProfileImage { get; set; }

        [Range(1, int.MaxValue)]
        public int DesignationId { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Range(0, 99999999)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
