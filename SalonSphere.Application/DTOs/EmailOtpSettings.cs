

namespace SalonSphere.Application.DTOs
{
    public class EmailOtpSettings
    {
        public int ExpiryMinutes { get; set; } = 5;
        public int ResendCooldownSeconds { get; set; } = 60;
        public int MaxAttempts { get; set; } = 5;
        public string SecretKey { get; set; } = string.Empty;
    }
}
