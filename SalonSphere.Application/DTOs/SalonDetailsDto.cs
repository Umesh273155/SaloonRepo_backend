
namespace SalonSphere.Application.DTOs
{
    public class SalonDetailsDto
    {
        public int SalonId { get; set; }
        public string SalonName { get; set; } = string.Empty;
        public string SalonType { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string? Website { get; set; }

        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public bool IsActive { get; set; }
    }
}
