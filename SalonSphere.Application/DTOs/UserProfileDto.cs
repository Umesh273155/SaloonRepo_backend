
namespace SalonSphere.Application.DTOs
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public int? SalonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
