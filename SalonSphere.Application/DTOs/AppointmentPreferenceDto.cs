
namespace SalonSphere.Application.DTOs
{
    public class AppointmentPreferenceDto
    {
        public int AppointmentPreferenceId { get; set; }
        public int AppointmentDuration { get; set; }
        public int AdvanceBookingDays { get; set; }
        public int CancellationHours { get; set; }
        public string TimeZone { get; set; } = "Asia/Kolkata";
        public bool OnlineBooking { get; set; }
        public bool RequireConfirmation { get; set; }
    }
}
