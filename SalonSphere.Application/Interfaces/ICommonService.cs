

using SalonSphere.Application.DTOs;

namespace SalonSphere.Application.Interfaces
{
    public interface ICommonService
    {
        Task<ApiResponse<List<ServiceCategoryResDto>>> GetServiceCategoriesAsync();
    }
}
