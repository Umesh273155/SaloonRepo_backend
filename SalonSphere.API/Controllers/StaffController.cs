
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;

namespace SalonSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        // POST: api/Staff
        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffReqDto request)
        {
            try
            {
                var response = await _staffService.CreateStaffAsync(request);

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

        // PUT: api/Staff/5
        [HttpPut("{staffId:int}")]
        public async Task<IActionResult> UpdateStaff(int staffId, [FromBody] UpdateStaffReqDto request)
        {
            try
            {
                var response = await _staffService.UpdateStaffAsync(staffId, request);

                if (!response.Success)
                {
                    if (response.Message == "Staff not found.")
                        return NotFound(response);

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

        // GET: api/Staff
        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            try
            {
                var response = await _staffService.GetAllStaffAsync();

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


        // GET: api/Staff/5
        [HttpGet("{staffId:int}")]
        public async Task<IActionResult> GetStaffById(int staffId)
        {
            try
            {
                var response = await _staffService.GetStaffByIdAsync(staffId);

                if (!response.Success)
                {
                    if (response.Message == "Staff not found.")
                        return NotFound(response);

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

        // DELETE: api/Staff/5
        [HttpDelete("{staffId:int}")]
        public async Task<IActionResult> DeleteStaff(int staffId)
        {
            try
            {
                var response = await _staffService.DeleteStaffAsync(staffId);

                if (!response.Success)
                {
                    if (response.Message == "Staff not found.")
                        return NotFound(response);

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

        // PATCH: api/Staff/5/status
        [HttpPatch("{staffId:int}/status")]
        public async Task<IActionResult> ActivateDeactivateStaff(int staffId,[FromQuery] bool isActive)
        {
            try
            {
                var response = await _staffService.ActivateDeactivateStaffAsync(staffId, isActive);

                if (!response.Success)
                {
                    if (response.Message == "Staff not found.")
                        return NotFound(response);

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
    }
}
