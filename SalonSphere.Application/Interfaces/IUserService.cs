

using SalonSphere.Application.DTOs;
using SalonSphere.Application.Helper;

namespace SalonSphere.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserProfileDto?>> GetMyProfileAsync(int userId);
        Task<ApiResponse<UserProfileDto?>> UpdateMyProfileAsync(int userId,UpdateUserProfileDto request);
        Task<ApiResponse<UserProfileDto?>> UpdateProfileImageAsync(int userId,byte[] imageBytes,string extension);
        Task<ApiResponse<UserProfileDto?>> RemoveProfileImageAsync(int userId);
    }
}
