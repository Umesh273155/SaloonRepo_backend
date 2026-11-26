

using SalonSphere.Application.DTOs;
namespace SalonSphere.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResDto>> RegisterAsync(RegisterReqDto request);
        Task<ApiResponse<LoginResDto>> LoginAsync(LoginReqDto req);
        Task<ApiResponse<object?>> ChangePasswordAsync(int userId, ChangePasswordReqDto req);
        // Email OTP Verification
        Task<ApiResponse<object?>> VerifyEmailAsync(VerifyEmailReqDto request);
        Task<ApiResponse<object?>> ResendOtpAsync(ResendOtpReqDto request);
        Task<ApiResponse<object?>> ForgotPasswordAsync(ForgotPasswordReqDto request);
        Task<ApiResponse<object?>> ResetPasswordAsync(ResetPasswordReqDto request);
    }
}
