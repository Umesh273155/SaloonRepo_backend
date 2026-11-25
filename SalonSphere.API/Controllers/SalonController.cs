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

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateSalon([FromBody] UpdateSalonReqDto request)
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
    }



}
