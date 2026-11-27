
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Helper;

namespace SalonSphere.Application.Interfaces
{
    public interface IStaffService
    {
        Task<ApiResponse<StaffResDto>> CreateStaffAsync(CreateStaffReqDto request);
        Task<ApiResponse<StaffResDto>> UpdateStaffAsync(int staffId,UpdateStaffReqDto request);
        Task<ApiResponse<IEnumerable<StaffResDto>>> GetAllStaffAsync();
        Task<ApiResponse<StaffResDto?>> GetStaffByIdAsync(int staffId);
        Task<ApiResponse<bool>> DeleteStaffAsync(int staffId);
        Task<ApiResponse<bool>> ActivateDeactivateStaffAsync(int staffId,bool isActive);
    }
}
