
namespace SalonSphere.Core.Entities
{
    public class Salon
    {
        public int SalonId { get; set; }
        public string SalonName { get; set; } = string.Empty;
        public string SalonType { get; set; } = "Unisex Salon";
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<SalonBusinessHour> BusinessHours { get; set; } = new List<SalonBusinessHour>();
        public AppointmentPreference? AppointmentPreference { get; set; }
    }
}
