
using System.ComponentModel.DataAnnotations;
namespace SalonSphere.Application.DTOs
{
    public class UpdateServiceReqDto
    {
        [Required]
        [StringLength(150)]
        public string ServiceName { get; set; } = string.Empty;

        [Required]        
        public int CategoryId { get; set; } 

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 1440)]
        public int DurationMinutes { get; set; }

        [Range(0, 99999999)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
