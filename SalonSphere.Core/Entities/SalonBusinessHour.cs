
namespace SalonSphere.Core.Entities
{
    public class SalonBusinessHour
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string Day { get; set; } = string.Empty;
        public bool IsOpen { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public Salon Salon { get; set; } = null!;
    }
}
