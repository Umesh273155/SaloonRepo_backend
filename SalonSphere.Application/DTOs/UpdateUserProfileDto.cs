
using System.ComponentModel.DataAnnotations;

namespace SalonSphere.Application.DTOs
{
    public class UpdateUserProfileDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }
    }

}
