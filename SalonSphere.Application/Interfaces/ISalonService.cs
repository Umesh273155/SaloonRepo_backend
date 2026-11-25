

using SalonSphere.Application.DTOs;

namespace SalonSphere.Application.Interfaces
{
    public interface ISalonService
    {
        Task<ApiResponse<SalonDetailsDto?>> GetMySalonAsync(int userId);
        Task<ApiResponse<SalonDetailsDto?>> UpdateSalonAsync(int userId, UpdateSalonReqDto request);
    }
}
