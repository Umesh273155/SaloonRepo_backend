
using System.ComponentModel.DataAnnotations;
namespace SalonSphere.Application.DTOs
{
    public class UpdateSalonReqDto
    {
        [Required]
        public string SalonName { get; set; } = string.Empty;
        public string SalonType { get; set; } = "Unisex Salon";
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? Website { get; set; }
        public string? OwnerName { get; set; }
    }
}
