

namespace SalonSphere.Core.Entities
{
    public class ServiceCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
