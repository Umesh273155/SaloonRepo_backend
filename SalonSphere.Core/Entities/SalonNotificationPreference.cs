

namespace SalonSphere.Core.Entities
{
    public class SalonNotificationPreference
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public bool AppointmentNotifications { get; set; } = true;
        public bool PaymentNotifications { get; set; } = true;
        public bool StockNotifications { get; set; } = true;
        public bool EmailNotifications { get; set; } = true;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
