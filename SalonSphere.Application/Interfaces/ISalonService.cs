

using SalonSphere.Application.DTOs;

namespace SalonSphere.Application.Interfaces
{
    public interface ISalonService
    {
        Task<ApiResponse<SalonDetailsDto?>> GetMySalonAsync(int userId);
        Task<ApiResponse<SalonDetailsDto?>> UpdateSalonAsync(int userId, UpdateSalonReqDto request);
        Task<ApiResponse<List<BusinessHoursDto>>> GetBusinessHoursAsync(int userId);
        Task<ApiResponse<bool>> UpdateBusinessHoursAsync(int userId,List<BusinessHoursDto> hours);
    }
}
