

namespace SalonSphere.Application.DTOs
{
    public class RegisterResDto
    {
        public int UserId { get; set; }
        public int SalonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SalonName { get; set; } = string.Empty;
    }
}
