

namespace SalonSphere.Core.Entities
{
    public class AppointmentPreference
    {
        public int AppointmentPreferenceId { get; set; }
        public int SalonId { get; set; }
        public int AppointmentDuration { get; set; } = 30;
        public int AdvanceBookingDays { get; set; } = 30;
        public int CancellationHours { get; set; } = 2;
        public string TimeZone { get; set; } = "Asia/Kolkata";
        public bool OnlineBooking { get; set; } = true;
        public bool RequireConfirmation { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Salon Salon { get; set; } = null!;
    }
}
