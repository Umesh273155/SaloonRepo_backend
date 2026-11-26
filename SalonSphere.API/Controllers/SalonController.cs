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
    public class SalonController : ControllerBase
    {
        private readonly ISalonService _salonService;
        public SalonController(ISalonService salonService)
        {
            _salonService = salonService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMySalon()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }
                var response = await _salonService.GetMySalonAsync(userId);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

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

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateSalon([FromBody] UpdateSalonReqDto request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonService.UpdateSalonAsync(userId, request);

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

        [HttpGet("business-hours")]
        public async Task<IActionResult> GetBusinessHours()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonService.GetBusinessHoursAsync(userId);
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

        [HttpPut("business-hours")]
        public async Task<IActionResult> UpdateBusinessHours([FromBody] List<BusinessHoursDto> hours)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonService.UpdateBusinessHoursAsync(userId, hours);

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


        [HttpGet("appointment-preferences")]
        public async Task<IActionResult> GetAppointmentPreferences()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                var response = await _salonService.GetAppointmentPreferencesAsync(userId);

                if (!response.Success)
                {
                    return BadRequest(response);
                }
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

        [HttpPut("appointment-preferences")]
        public async Task<IActionResult> UpdateAppointmentPreferences([FromBody] UpdateAppointmentPreferenceDto request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user authentication."
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _salonService.UpdateAppointmentPreferencesAsync(userId, request);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

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


        [HttpGet("notification-preferences")]
        public async Task<IActionResult> GetNotificationPreferences()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user token"
                    });
                }

                var result = await _salonService.GetNotificationPreferencesAsync(userId);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
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


        [HttpPut("notification-preferences")]
        public async Task<IActionResult> UpdateNotificationPreferences([FromBody] UpdateNotificationPreferencesReqDto request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        Success = false,
                        Message = "Invalid user token"
                    });
                }

                var result = await _salonService.UpdateNotificationPreferencesAsync(userId, request);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
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
