
using System.ComponentModel.DataAnnotations;

namespace SalonSphere.Application.DTOs
{
    public class ResendOtpReqDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
