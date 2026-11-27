
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Helper;
using SalonSphere.Application.Interfaces;

namespace SalonSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IWebHostEnvironment _environment;
        private const long MaxFileSize = 2 * 1024 * 1024;      

        public StaffController(IStaffService staffService,IWebHostEnvironment environment)
        {
            _staffService = staffService;
            _environment = environment;
        }

        // POST: api/Staff
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateStaff([FromForm] CreateStaffReqDto request,[FromForm] IFormFile? profilePhoto)
        {
            string? savedImagePath = null;

            try
            {
                if (profilePhoto != null)
                {
                    var validationError = ValidatePhoto(profilePhoto);

                    if (validationError != null)
                        return BadRequest(new
                        {
                            Success = false,
                            Message = validationError
                        });

                    savedImagePath = await SavePhotoAsync(profilePhoto);
                }

                // Ignore any ProfileImage value supplied by the client.
                request.ProfileImage = savedImagePath;

                var response = await _staffService.CreateStaffAsync(request);

                if (!response.Success)
                {
                    DeletePhoto(savedImagePath);
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                DeletePhoto(savedImagePath);

                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        // PUT: api/Staff/5
        [HttpPut("{staffId:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateStaff(int staffId,[FromForm] UpdateStaffReqDto request,[FromForm] IFormFile? profilePhoto)
        {
            string? newImagePath = null;

            try
            {
                // Get existing image before updating.
                var existingResponse = await _staffService.GetStaffByIdAsync(staffId);

                if (!existingResponse.Success || existingResponse.Data == null)
                    return NotFound(existingResponse);

                var oldImagePath = existingResponse.Data.ProfileImage;

                if (profilePhoto != null)
                {
                    var validationError = ValidatePhoto(profilePhoto);

                    if (validationError != null)
                        return BadRequest(new
                        {
                            Success = false,
                            Message = validationError
                        });

                    newImagePath = await SavePhotoAsync(profilePhoto);
                }

                // No new photo selected: preserve the existing image.
                request.ProfileImage = newImagePath;

                var response =
                    await _staffService.UpdateStaffAsync(staffId, request);

                if (!response.Success)
                {
                    DeletePhoto(newImagePath);

                    if (response.Message == "Staff not found.")
                        return NotFound(response);

                    return BadRequest(response);
                }

                // Delete old image only after successful update.
                if (newImagePath != null)
                    DeletePhoto(oldImagePath);

                return Ok(response);
            }
            catch (Exception ex)
            {
                DeletePhoto(newImagePath);

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
                var existingResponse =
                    await _staffService.GetStaffByIdAsync(staffId);

                if (!existingResponse.Success || existingResponse.Data == null)
                    return NotFound(existingResponse);

                var oldImagePath = existingResponse.Data.ProfileImage;

                var response = await _staffService.DeleteStaffAsync(staffId);

                if (!response.Success)
                {
                    if (response.Message == "Staff not found.")
                        return NotFound(response);

                    return BadRequest(response);
                }

                DeletePhoto(oldImagePath);

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

        // PATCH: api/Staff/5/status?isActive=true
        [HttpPatch("{staffId:int}/status")]
        public async Task<IActionResult> ActivateDeactivateStaff(int staffId,[FromQuery] bool isActive)
        {
            try
            {
                var response =
                    await _staffService.ActivateDeactivateStaffAsync(
                        staffId, isActive);

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

        // Validate uploaded photo
        private static string? ValidatePhoto(IFormFile photo)
        {
            if (photo.Length == 0)
                return "Please select a valid image.";

            if (photo.Length > MaxFileSize)
                return "Image size cannot exceed 2 MB.";

            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

            if (!AllowedImageExtension.IsValid(photo.FileName))
            {               
                return "Only JPG, JPEG, PNG and WEBP images are allowed.";
            }
            return null;
        }


        // Save image inside wwwroot/uploads/staff
        private async Task<string> SavePhotoAsync(IFormFile photo)
        {
            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

            var uploadFolder = Path.Combine(_environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"),"uploads","staff");

            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return $"/uploads/staff/{fileName}";
        }

        // Delete only files from staff upload folder
        private void DeletePhoto(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            var fileName = Path.GetFileName(imagePath);

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var uploadFolder = Path.Combine(
                _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"),
                "uploads",
                "staff"
            );

            var filePath = Path.Combine(uploadFolder, fileName);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
