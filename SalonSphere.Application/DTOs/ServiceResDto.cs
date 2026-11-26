
namespace SalonSphere.Application.DTOs
{
    public class ServiceResDto
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
