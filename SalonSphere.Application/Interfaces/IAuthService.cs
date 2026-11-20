

using SalonSphere.Application.DTOs;
namespace SalonSphere.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResDto>> RegisterAsync(RegisterReqDto request);
        Task<ApiResponse<LoginResDto>> LoginAsync(LoginReqDto request);
    }
}
