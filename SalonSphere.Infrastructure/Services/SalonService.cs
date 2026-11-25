using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Infrastructure.Data;

namespace SalonSphere.Infrastructure.Services
{
    public class SalonService:ISalonService
    {
        private readonly ApplicationDbContext _context;

        public SalonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<SalonDetailsDto?>> GetMySalonAsync(int userId)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }
            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }
            var salon = await _context.Salons.AsNoTracking().Where(s => s.SalonId == user.SalonId.Value)
                .Select(s=>new SalonDetailsDto
                {
                    SalonId = s.SalonId,
                    SalonName = s.SalonName,
                    SalonType= s.SalonType,
                    Description = s.Description,
                    LogoUrl = s.LogoUrl,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address,
                    City = s.City,
                    State = s.State,
                    Pincode = s.Pincode,
                    IsActive = s.IsActive,
                    Website = s.Website,
                }).FirstOrDefaultAsync();

            if (salon != null)
            {
                salon.OwnerName = user.FullName;
            }

            if (salon == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "Salon not found.",
                    Data = null
                };
            }

            return new ApiResponse<SalonDetailsDto?>
            {
                Success = true,
                Message = "Salon details fetched successfully.",
                Data = salon
            };
        }

        public async Task<ApiResponse<SalonDetailsDto?>> UpdateSalonAsync(int userId, UpdateSalonReqDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }

            var salon = await _context.Salons.FirstOrDefaultAsync(s => s.SalonId == user.SalonId.Value);
            if (salon == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "Salon not found.",
                    Data = null
                };
            }

            salon.SalonName = request.SalonName;
            salon.SalonType = request.SalonType;
            salon.Description = request.Description;
            salon.Phone = request.Phone;
            salon.Email = request.Email;
            salon.Address = request.Address;
            salon.City = request.City;
            salon.State = request.State;
            salon.Pincode = request.Pincode;
            salon.Website = request.Website;
            user.FullName = request.OwnerName ?? user.FullName;

            await _context.SaveChangesAsync();
            var response = new SalonDetailsDto
            {
                SalonId = salon.SalonId,
                SalonName = salon.SalonName,
                SalonType = salon.SalonType,
                Description = salon.Description,
                LogoUrl = salon.LogoUrl,
                Phone = salon.Phone,
                Email = salon.Email,
                Address = salon.Address,
                City = salon.City,
                State = salon.State,
                Pincode = salon.Pincode,
                IsActive = salon.IsActive,
                Website = salon.Website,
                OwnerName = user.FullName,               
            };

            return new ApiResponse<SalonDetailsDto?>
            {
                Success = true,
                Message = "Salon details updated successfully.",
                Data = response
            };
        }
    }
}
