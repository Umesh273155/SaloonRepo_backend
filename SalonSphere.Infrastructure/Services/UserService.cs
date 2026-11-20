

using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Infrastructure.Data;
namespace SalonSphere.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<UserProfileDto?>> GetMyProfileAsync(int userId)
        {
            var profile = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new UserProfileDto
                {
                    UserId = u.UserId,
                    SalonId = u.SalonId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.Role.RoleName,
                    ProfileImage = u.ProfileImage,
                })
                .FirstOrDefaultAsync();

            if (profile == null)
            {
                return new ApiResponse<UserProfileDto?>
                {
                    Success = false,
                    Message = "User profile not found.",
                    Data = null
                };
            }

            return new ApiResponse<UserProfileDto?>
            {
                Success = true,
                Message = "Profile fetched successfully.",
                Data = profile
            };
        }

        public async Task<ApiResponse<UserProfileDto?>> UpdateMyProfileAsync(int userId, UpdateUserProfileDto request)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return new ApiResponse<UserProfileDto?>
                {
                    Success = false,
                    Message = "User profile not found.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return new ApiResponse<UserProfileDto?>
                {
                    Success = false,
                    Message = "Full name is required.",
                    Data = null
                };
            }

            user.FullName = request.FullName.Trim();
            user.Phone = string.IsNullOrWhiteSpace(request.Phone)? null: request.Phone.Trim();

            await _context.SaveChangesAsync();
            var profile = new UserProfileDto
            {
                UserId = user.UserId,
                SalonId = user.SalonId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                ProfileImage = user.ProfileImage,
                Role = user.Role?.RoleName ?? string.Empty
            };

            return new ApiResponse<UserProfileDto?>
            {
                Success = true,
                Message = "Profile updated successfully.",
                Data = profile
            };
        }
    }
}