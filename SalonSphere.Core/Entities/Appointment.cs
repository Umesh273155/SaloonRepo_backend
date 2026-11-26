
namespace SalonSphere.Core.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int SalonId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int ServiceId { get; set; }
        public int StaffId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Salon Salon { get; set; } = null!;
        public Service Service { get; set; } = null!;
        public Staff Staff { get; set; } = null!;
    }
}
