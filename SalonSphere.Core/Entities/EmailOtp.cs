

namespace SalonSphere.Core.Entities
{
    public class EmailOtp
    {
        public int EmailOtpId { get; set; }
        public int UserId { get; set; }
        public string OtpHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int Attempts { get; set; } = 0;
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
    }
}
