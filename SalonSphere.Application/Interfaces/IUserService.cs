

using SalonSphere.Application.DTOs;

namespace SalonSphere.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserProfileDto?>> GetMyProfileAsync(int userId);
        Task<ApiResponse<UserProfileDto?>> UpdateMyProfileAsync(int userId,UpdateUserProfileDto request);
    }
}
