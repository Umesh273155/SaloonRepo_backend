
using System.ComponentModel.DataAnnotations;

namespace SalonSphere.Application.DTOs
{
    public class VerifyEmailReqDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{6}$",ErrorMessage = "OTP must be exactly 6 digits.")]
        public string Otp { get; set; } = string.Empty;
    }
}
