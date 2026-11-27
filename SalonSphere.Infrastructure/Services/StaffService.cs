using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Core.Entities;
using SalonSphere.Infrastructure.Data;
using System.Security.Claims;

namespace SalonSphere.Infrastructure.Services
{
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StaffService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // CREATE STAFF
        public async Task<ApiResponse<StaffResDto>> CreateStaffAsync(CreateStaffReqDto request)
        {
            var salonId = await GetSalonIdAsync();

            if (salonId == null)
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Salon not found for logged-in user."
                };
            }

            if (await IsDuplicateStaffAsync(salonId.Value, request.Email, request.Phone))
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Staff with this email or phone already exists."
                };
            }

            if (!await IsValidDesignationAsync(request.DesignationId))
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Invalid designation selected."
                };
            }

            var staff = new Staff
            {
                SalonId = salonId.Value,
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone.Trim(),
                ProfileImage = request.ProfileImage,
                DesignationId = request.DesignationId,
                JoiningDate = request.JoiningDate,
                Salary = request.Salary,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.StaffMembers.Add(staff);

            await _context.SaveChangesAsync();

            await _context.Entry(staff).Reference(x => x.Designation).LoadAsync();

            return new ApiResponse<StaffResDto>
            {
                Success = true,
                Message = "Staff created successfully.",
                Data = MapToDto(staff)
            };
        }

        // UPDATE STAFF
        public async Task<ApiResponse<StaffResDto>> UpdateStaffAsync(int staffId, UpdateStaffReqDto request)
        {
            var salonId = await GetSalonIdAsync();

            if (salonId == null)
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Salon not found."
                };
            }

            var staff = await _context.StaffMembers
                .Include(x => x.Designation)
                .FirstOrDefaultAsync(x => x.StaffId == staffId && x.SalonId == salonId.Value);

            if (staff == null)
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Staff not found."
                };
            }

            if (await IsDuplicateStaffAsync(salonId.Value, request.Email, request.Phone, staffId))
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Email or phone already exists for another staff."
                };
            }

            if (!await IsValidDesignationAsync(request.DesignationId))
            {
                return new ApiResponse<StaffResDto>
                {
                    Success = false,
                    Message = "Invalid designation selected."
                };
            }

            staff.FullName = request.FullName.Trim();
            staff.Email = request.Email.Trim();
            staff.Phone = request.Phone.Trim();
            staff.ProfileImage = request.ProfileImage;
            staff.DesignationId = request.DesignationId;
            staff.JoiningDate = request.JoiningDate;
            staff.Salary = request.Salary;
            staff.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            await _context.Entry(staff).Reference(x => x.Designation).LoadAsync();

            return new ApiResponse<StaffResDto>
            {
                Success = true,
                Message = "Staff updated successfully.",
                Data = MapToDto(staff)
            };
        }

        // GET ALL STAFF
        public async Task<ApiResponse<IEnumerable<StaffResDto>>> GetAllStaffAsync()
        {
            var salonId = await GetSalonIdAsync();

            if (salonId == null)
            {
                return new ApiResponse<IEnumerable<StaffResDto>>
                {
                    Success = false,
                    Message = "Salon not found."
                };
            }

            var staffList = await _context.StaffMembers
                .AsNoTracking()
                .Include(x => x.Designation)
                .Where(x => x.SalonId == salonId.Value)
                .OrderByDescending(x => x.StaffId)
                .ToListAsync();

            var result = staffList.Select(MapToDto);

            return new ApiResponse<IEnumerable<StaffResDto>>
            {
                Success = true,
                Message = "Staff retrieved successfully.",
                Data = result
            };
        }

        // GET STAFF BY ID
        public async Task<ApiResponse<StaffResDto?>> GetStaffByIdAsync(int staffId)
        {
            var salonId = await GetSalonIdAsync();

            if (salonId == null)
            {
                return new ApiResponse<StaffResDto?>
                {
                    Success = false,
                    Message = "Salon not found."
                };
            }

            var staff = await _context.StaffMembers
                .AsNoTracking()
                .Include(x => x.Designation)
                .FirstOrDefaultAsync(x => x.StaffId == staffId && x.SalonId == salonId.Value);

            if (staff == null)
            {
                return new ApiResponse<StaffResDto?>
                {
                    Success = false,
                    Message = "Staff not found."
                };
            }

            return new ApiResponse<StaffResDto?>
            {
                Success = true,
                Message = "Staff retrieved successfully.",
                Data = MapToDto(staff)
            };
        }

        // DELETE STAFF
        public async Task<ApiResponse<bool>> DeleteStaffAsync(int staffId)
        {
            var salonId = await GetSalonIdAsync();

            if (salonId == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Salon not found.",
                    Data = false
                };
            }

            var staff = await _context.StaffMembers.FirstOrDefaultAsync(x => x.StaffId == staffId && x.SalonId == salonId.Value);

            if (staff == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Staff not found.",
                    Data = false
                };
            }

            _context.StaffMembers.Remove(staff);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Staff deleted successfully.",
                Data = true
            };
        }

        // ACTIVATE / DEACTIVATE STAFF
        public async Task<ApiResponse<bool>> ActivateDeactivateStaffAsync(int staffId, bool isActive)
        {
            var salonId = await GetSalonIdAsync();

            if (salonId == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Salon not found.",
                    Data = false
                };
            }

            var staff = await _context.StaffMembers.FirstOrDefaultAsync(x => x.StaffId == staffId && x.SalonId == salonId.Value);

            if (staff == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Staff not found.",
                    Data = false
                };
            }

            staff.IsActive = isActive;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = isActive ? "Staff activated successfully." : "Staff deactivated successfully.",
                Data = true
            };
        }

        // Check Duplicate Email / Phone
        private async Task<bool> IsDuplicateStaffAsync(int salonId, string email, string phone, int? excludeStaffId = null)
        {
            return await _context.StaffMembers.AnyAsync(x => x.SalonId == salonId && x.StaffId != (excludeStaffId ?? 0) && (x.Email == email || x.Phone == phone));
        }

        // Validate Designation
        private async Task<bool> IsValidDesignationAsync(int designationId)
        {
            return await _context.Designations.AnyAsync(x => x.DesignationId == designationId && x.IsActive);
        }

        // Get SalonId from logged-in user
        private async Task<int?> GetSalonIdAsync()
        {
            var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out int userId))
                return null;

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

            return user?.SalonId;
        }

        // Map Entity to Response DTO
        private static StaffResDto MapToDto(Staff staff)
        {
            return new StaffResDto
            {
                StaffId = staff.StaffId,
                FullName = staff.FullName,
                Email = staff.Email,
                Phone = staff.Phone,
                ProfileImage = staff.ProfileImage,
                DesignationId = staff.DesignationId,
                DesignationName = staff.Designation?.DesignationName ?? "",
                JoiningDate = staff.JoiningDate,
                Salary = staff.Salary,
                IsActive = staff.IsActive,
                CreatedAt = staff.CreatedAt
            };
        }

    }
}
