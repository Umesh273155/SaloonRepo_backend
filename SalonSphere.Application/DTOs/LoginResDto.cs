
namespace SalonSphere.Application.DTOs
{
    public class LoginResDto
    {
        public int UserId { get; set; }
        public int? SalonId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public DateTime? ExpiresAt { get; set; }
    }
}
