using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using System.Security.Claims;

namespace SalonSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalonOfferingController : ControllerBase
    {
        private readonly ISalonOfferingService _salonOfferingService;
        public SalonOfferingController(ISalonOfferingService salonOfferingService)
        {
            _salonOfferingService = salonOfferingService;
        }
   
        private bool TryGetUserId(out int userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out userId);
        }


        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceReqDto request)
        {
            try
            {
                if (!TryGetUserId(out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonOfferingService.CreateServiceAsync(request);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{serviceId:int}")]
        public async Task<IActionResult> UpdateService(int serviceId,[FromBody] UpdateServiceReqDto request)
        {
            try
            {
                if (!TryGetUserId(out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonOfferingService.UpdateServiceAsync(serviceId, request);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                if (!TryGetUserId(out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonOfferingService
                    .GetAllServicesAsync();

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{serviceId:int}")]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            try
            {
                if (!TryGetUserId(out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonOfferingService
                    .GetServiceByIdAsync(serviceId);

                if (!response.Success)
                    return NotFound(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPatch("{serviceId:int}/status")]
        public async Task<IActionResult> ActivateDeactivateService(int serviceId,[FromQuery] bool isActive)
        {
            try
            {
                if (!TryGetUserId(out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonOfferingService.ActivateDeactivateServiceAsync(serviceId, isActive);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

    }
}
