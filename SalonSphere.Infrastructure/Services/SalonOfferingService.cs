
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Core.Entities;
using SalonSphere.Infrastructure.Data;

namespace SalonSphere.Infrastructure.Services
{
    public class SalonOfferingService : ISalonOfferingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalonOfferingService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Get Logged-in User's SalonId
        private async Task<int> GetSalonIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                throw new UnauthorizedAccessException("Invalid user authentication.");

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (!user.SalonId.HasValue)
                throw new InvalidOperationException("No salon is associated with this user.");

            return user.SalonId.Value;
        }

        public async Task<ApiResponse<ServiceResDto>> CreateServiceAsync(CreateServiceReqDto request)
        {
            try
            {
                var salonId = await GetSalonIdAsync();

                var category = await _context.ServiceCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId && x.IsActive);

                if (category == null)
                {
                    return new ApiResponse<ServiceResDto>
                    {
                        Success = false,
                        Message = "Invalid or inactive service category."
                    };
                }

                var service = new Service
                {
                    SalonId = salonId,
                    ServiceName = request.ServiceName,
                    CategoryId = category.CategoryId,
                    Category = category,
                    Description = request.Description,
                    DurationMinutes = request.DurationMinutes,
                    Price = request.Price,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                return new ApiResponse<ServiceResDto>
                {
                    Success = true,
                    Message = "Service created successfully.",
                    Data = MapToDto(service)
                };
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<ServiceResDto>
                {
                    Success = false,
                    Message = ex.GetBaseException().Message
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ServiceResDto>
                {
                    Success = false,
                    Message = ex.GetBaseException().Message
                };
            }
            
        }

        // Update Service
        public async Task<ApiResponse<ServiceResDto>> UpdateServiceAsync(int serviceId, UpdateServiceReqDto request)
        {
            try
            {
                var salonId = await GetSalonIdAsync();

                var service = await _context.Services.FirstOrDefaultAsync(x => x.ServiceId == serviceId && x.SalonId == salonId);

                if (service == null)
                {
                    return new ApiResponse<ServiceResDto>
                    {
                        Success = false,
                        Message = "Service not found."
                    };
                }

                var category = await _context.ServiceCategories.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId && x.IsActive);

                if (category == null)
                {
                    return new ApiResponse<ServiceResDto>
                    {
                        Success = false,
                        Message = "Invalid or inactive service category."
                    };
                }

                service.ServiceName = request.ServiceName;
                service.CategoryId = category.CategoryId;
                service.Category = category;
                service.Description = request.Description;
                service.DurationMinutes = request.DurationMinutes;
                service.Price = request.Price;
                service.IsActive = request.IsActive;

                await _context.SaveChangesAsync();

                return new ApiResponse<ServiceResDto>
                {
                    Success = true,
                    Message = "Service updated successfully.",
                    Data = MapToDto(service)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ServiceResDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // Get All Services
        public async Task<ApiResponse<IEnumerable<ServiceResDto>>> GetAllServicesAsync()
        {
            try
            {
                var salonId = await GetSalonIdAsync();

                var services = await _context.Services
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Where(x => x.SalonId == salonId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                return new ApiResponse<IEnumerable<ServiceResDto>>
                {
                    Success = true,
                    Message = "Services retrieved successfully.",
                    Data = services.Select(MapToDto).ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ServiceResDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // Get Service By Id
        public async Task<ApiResponse<ServiceResDto?>> GetServiceByIdAsync(int serviceId)
        {
            try
            {
                var salonId = await GetSalonIdAsync();

                var service = await _context.Services
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.ServiceId == serviceId && x.SalonId == salonId);

                if (service == null)
                {
                    return new ApiResponse<ServiceResDto?>
                    {
                        Success = false,
                        Message = "Service not found."
                    };
                }

                return new ApiResponse<ServiceResDto?>
                {
                    Success = true,
                    Message = "Service retrieved successfully.",
                    Data = MapToDto(service)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ServiceResDto?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // Activate / Deactivate Service
        public async Task<ApiResponse<bool>> ActivateDeactivateServiceAsync(int serviceId, bool isActive)
        {
            try
            {
                var salonId = await GetSalonIdAsync();

                var service = await _context.Services.FirstOrDefaultAsync(x => x.ServiceId == serviceId && x.SalonId == salonId);

                if (service == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Service not found.",
                        Data = false
                    };
                }

                service.IsActive = isActive;

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = isActive ? "Service activated successfully." : "Service deactivated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        // Entity To DTO Mapping
        private static ServiceResDto MapToDto(Service service)
        {
            return new ServiceResDto
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                CategoryId = service.CategoryId,
                CategoryName = service.Category?.CategoryName ?? string.Empty,
                Description = service.Description,
                DurationMinutes = service.DurationMinutes,
                Price = service.Price,
                IsActive = service.IsActive
            };
        }
    }
}
