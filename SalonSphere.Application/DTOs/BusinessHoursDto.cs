

namespace SalonSphere.Application.DTOs
{
    public class BusinessHoursDto
    {
        public string Day { get; set; } = string.Empty;
        public bool IsOpen { get; set; }
        public string OpenTime { get; set; } = "09:00";
        public string CloseTime { get; set; } = "20:00";
    }
}
