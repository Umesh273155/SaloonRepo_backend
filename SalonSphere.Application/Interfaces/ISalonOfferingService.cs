
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Helper;

namespace SalonSphere.Application.Interfaces
{
    public interface ISalonOfferingService
    {
        Task<ApiResponse<ServiceResDto>> CreateServiceAsync(CreateServiceReqDto request);
        Task<ApiResponse<ServiceResDto>> UpdateServiceAsync(int serviceId, UpdateServiceReqDto request);
        Task<ApiResponse<IEnumerable<ServiceResDto>>> GetAllServicesAsync();
        Task<ApiResponse<ServiceResDto?>> GetServiceByIdAsync(int serviceId);
        Task<ApiResponse<bool>> ActivateDeactivateServiceAsync(int serviceId, bool isActive);
    }
}
