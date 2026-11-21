
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid user authentication.",
                    Data = (object?)null
                });
            }
            var response = await _userService.GetMyProfileAsync(userId);
            
            if (!response.Success)
            {
                return NotFound(response);
            }

            var baseUrl = _config["AppSettings:BaseUrl"]?.TrimEnd('/');
            if (!string.IsNullOrEmpty(response.Data?.ProfileImage))
            {
                response.Data.ProfileImage = $"{baseUrl}{response.Data.ProfileImage}";
            }
            return Ok(response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid user authentication.",
                    Data = (object?)null
                });
            }
          
            var response = await _userService.UpdateMyProfileAsync(userId, request);
           
            if (!response.Success)
            {
                return NotFound(response);
            }
          
            return Ok(response);
        }


        [HttpPost("profile/photo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfilePhoto([FromForm(Name = "file")] IFormFile? file)
        {
            var form = await Request.ReadFormAsync();

            Console.WriteLine($"Form Files Count: {form.Files.Count}");

            foreach (var uploadedFile in form.Files)
            {
                Console.WriteLine($"Key: {uploadedFile.Name}");
                Console.WriteLine($"File: {uploadedFile.FileName}");
                Console.WriteLine($"Length: {uploadedFile.Length}");
            }

            Console.WriteLine($"Parameter Null: {file == null}");
            Console.WriteLine($"Parameter Length: {file?.Length ?? 0}");
            if (file == null || file.Length == 0)
            {
                file = Request.Form.Files.GetFile("file");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Please select an image."
                });
            }



            if (Request.HasFormContentType)
            {
                foreach (var item in Request.Form.Files)
                {
                    Console.WriteLine($"File Name: {item.FileName}");
                    Console.WriteLine($"File Length: {item.Length}");
                }
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid user authentication."
                });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Please select an image."
                });
            }

            if (file.Length > 2 * 1024 * 1024)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Image size cannot exceed 2 MB."
                });
            }
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Only JPG, JPEG, PNG and WEBP images are allowed."
                });
            }
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            if (!IsValidImage(imageBytes, extension))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid image file."
                });
            }

            var response = await _userService.UpdateProfileImageAsync(userId, imageBytes, extension);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        private static bool IsValidImage(byte[] bytes, string extension)
        {
            if (extension == ".jpg" || extension == ".jpeg")
            {
                return bytes.Length >= 3 &&
                       bytes[0] == 0xFF &&
                       bytes[1] == 0xD8 &&
                       bytes[2] == 0xFF;
            }

            if (extension == ".png")
            {
                return bytes.Length >= 8 &&
                       bytes[0] == 0x89 &&
                       bytes[1] == 0x50 &&
                       bytes[2] == 0x4E &&
                       bytes[3] == 0x47 &&
                       bytes[4] == 0x0D &&
                       bytes[5] == 0x0A &&
                       bytes[6] == 0x1A &&
                       bytes[7] == 0x0A;
            }

            if (extension == ".webp")
            {
                return bytes.Length >= 12 &&
                       System.Text.Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF" &&
                       System.Text.Encoding.ASCII.GetString(bytes, 8, 4) == "WEBP";
            }

            return false;
        }

    }
}
