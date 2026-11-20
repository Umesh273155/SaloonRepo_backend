using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;

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
}