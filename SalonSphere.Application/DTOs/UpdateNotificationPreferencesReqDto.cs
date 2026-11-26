

namespace SalonSphere.Application.DTOs
{
    public class UpdateNotificationPreferencesReqDto
    {
        public bool AppointmentNotifications { get; set; }
        public bool PaymentNotifications { get; set; }
        public bool StockNotifications { get; set; }
        public bool EmailNotifications { get; set; }
    }
}
