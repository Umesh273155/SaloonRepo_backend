
using System.ComponentModel.DataAnnotations;

namespace SalonSphere.Application.DTOs
{
    public class ChangePasswordReqDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(NewPassword),ErrorMessage = "New password and confirm password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
