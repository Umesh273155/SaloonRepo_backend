

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Helper;
using SalonSphere.Application.Interfaces;
using SalonSphere.Infrastructure.Data;
namespace SalonSphere.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public UserService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
            user.Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim();

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

        public async Task<ApiResponse<UserProfileDto?>> UpdateProfileImageAsync(int userId, byte[] imageBytes, string extension)
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

            var folderPath = Path.Combine(_environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"), "uploads");

            Directory.CreateDirectory(folderPath);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(folderPath, fileName);
            await File.WriteAllBytesAsync(filePath, imageBytes);

            var oldImage = user.ProfileImage;

            user.ProfileImage = $"/uploads/{fileName}";

            await _context.SaveChangesAsync();

            // Delete previous profile image if it belongs to this upload folder.
            if (!string.IsNullOrWhiteSpace(oldImage))
            {
                var oldFileName = Path.GetFileName(oldImage);
                var oldFilePath = Path.Combine(folderPath, oldFileName);

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

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
                Message = "Profile image updated successfully.",
                Data = profile
            };
        }

        public async Task<ApiResponse<UserProfileDto?>> RemoveProfileImageAsync(int userId)
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

            var oldImage = user.ProfileImage;

            // Remove profile image path from database
            user.ProfileImage = null;

            await _context.SaveChangesAsync();

            // Delete old image from uploads folder
            if (!string.IsNullOrWhiteSpace(oldImage))
            {
                var webRootPath = _environment.WebRootPath?? Path.Combine(_environment.ContentRootPath, "wwwroot");
                var folderPath = Path.Combine(webRootPath, "uploads");
                // Handle both relative and full image URLs
                var imageFileName = Path.GetFileName(Uri.TryCreate(oldImage, UriKind.Absolute, out var uri)
                        ? uri.LocalPath: oldImage);
                var oldFilePath = Path.Combine(folderPath, imageFileName);

                Console.WriteLine($"Deleting Image: {oldFilePath}");

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                    Console.WriteLine("Image deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Image file not found at this location.");
                }
            }

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
                Message = "Profile image removed successfully.",
                Data = profile
            };
        }
    }
}