using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Infrastructure.Data;

namespace SalonSphere.Infrastructure.Services
{
    public class CommonService : ICommonService
    {
        private readonly ApplicationDbContext _context;

        public CommonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ServiceCategoryResDto>>> GetServiceCategoriesAsync()
        {
            var categories = await _context.ServiceCategories.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.CategoryName)
                .Select(x => new ServiceCategoryResDto
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return new ApiResponse<List<ServiceCategoryResDto>>
            {
                Success = true,
                Message = "Service categories fetched successfully.",
                Data = categories
            };
        }
    }
}