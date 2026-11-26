

namespace SalonSphere.Core.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int SalonId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Salon Salon { get; set; } = null!;        
        public ServiceCategory Category { get; set; } = null!;

    }
}
