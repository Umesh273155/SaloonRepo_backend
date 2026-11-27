

using SalonSphere.Application.DTOs;
using SalonSphere.Application.Helper;

namespace SalonSphere.Application.Interfaces
{
    public interface ICommonService
    {
        Task<ApiResponse<List<ServiceCategoryResDto>>> GetServiceCategoriesAsync();
        Task<ApiResponse<IEnumerable<DesignationResDto>>> GetDesignationsAsync();
    }
}
