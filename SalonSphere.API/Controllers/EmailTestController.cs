using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalonSphere.Application.Interfaces;

namespace SalonSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail([FromBody] SendTestEmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToEmail))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Recipient email is required."
                });
            }

            try
            {
                string htmlMessage = $@"
                    <div style='font-family:Arial;padding:20px;'>
                        <h2 style='color:#7c3aed;'>Welcome to SalonSphere!</h2>
                        <p>This is a test email from SalonSphere.</p>
                        <p>Your email service is working successfully.</p>
                        <br/>
                        <strong>Regards,</strong>
                        <p>SalonSphere Team</p>
                    </div>";

                await _emailService.SendEmailAsync(
                    request.ToEmail,
                    "SalonSphere - Test Email",
                    htmlMessage
                );

                return Ok(new
                {
                    success = true,
                    message = "Test email sent successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to send test email.",
                    error = ex.Message
                });
            }
        }
        public class SendTestEmailRequest
        {
            public string ToEmail { get; set; } = string.Empty;
        }

    }
}
