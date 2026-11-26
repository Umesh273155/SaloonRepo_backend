
using System.ComponentModel.DataAnnotations;

namespace SalonSphere.Application.DTOs
{
    public class UpdateAppointmentPreferenceDto
    {
        [Required]
        [Range(15, 90)]
        public int AppointmentDuration { get; set; }
        [Required]
        [Range(7, 90)]
        public int AdvanceBookingDays { get; set; }
        [Required]
        [Range(1, 24)]
        public int CancellationHours { get; set; }
        [Required]
        [MaxLength(100)]
        public string TimeZone { get; set; } = "Asia/Kolkata";
        public bool OnlineBooking { get; set; }
        public bool RequireConfirmation { get; set; }
    }
}

