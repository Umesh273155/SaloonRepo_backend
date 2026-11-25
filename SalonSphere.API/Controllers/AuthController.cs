using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using System.Security.Claims;

namespace SalonSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterReqDto request)
    {
        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            if (response.Message == "This email is already registered.")
                return Conflict(response);

            return BadRequest(response);
        }

        return StatusCode(201, response);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginReqDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }


    // POST: api/auth/forgot-password
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordReqDto request)
    {
        var response = await _authService.ForgotPasswordAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    // POST: api/auth/reset-password
    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReqDto request)
    {
        var response = await _authService.ResetPasswordAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }



    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordReqDto request)
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

        var response = await _authService.ChangePasswordAsync(userId,request);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    // POST: api/auth/verify-email
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailReqDto request)
    {
        var response = await _authService.VerifyEmailAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    // POST: api/auth/resend-otp
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpReqDto request)
    {
        var response = await _authService.ResendOtpAsync(request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }
}