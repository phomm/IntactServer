using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Intact.BusinessLogic.Services;

namespace Intact.API.Controllers;

/// <summary>
/// Controller for testing email functionality in development environment
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmailTestController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailTestController> _logger;
    private readonly IWebHostEnvironment _environment;

    public EmailTestController(
        IEmailService emailService, 
        ILogger<EmailTestController> logger,
        IWebHostEnvironment environment)
    {
        _emailService = emailService;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Test email sending functionality (Development only)
    /// </summary>
    [HttpPost("send-test")]
    public async Task<IActionResult> SendTestEmail([FromBody] TestEmailRequest request)
    {
        // Only allow in development environment
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        try
        {
            await _emailService.SendEmailAsync(
                request.To, 
                request.Subject ?? "🧪 Test Email from Intact API", 
                request.Body ?? "<h2>Test Email</h2><p>This is a test email from the Intact application using Apprise email service.</p>"
            );

            _logger.LogInformation("Test email sent successfully to {Email}", request.To);
            
            return Ok(new { 
                success = true, 
                message = $"Test email sent successfully to {request.To}",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send test email to {Email}", request.To);
            
            return BadRequest(new { 
                success = false, 
                message = $"Failed to send test email: {ex.Message}",
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Test email confirmation template (Development only)
    /// </summary>
    [HttpPost("test-confirmation")]
    public async Task<IActionResult> TestEmailConfirmation([FromBody] ConfirmationTestRequest request)
    {

        try
        {
            await _emailService.SendConfirmationLinkAsync(
                GetTestUser(request.Email),
                request.UserName,
                request.ConfirmationLink ?? "https://localhost:7000/confirmEmail?userId=test&code=testcode"
            );

            return Ok(new { 
                success = true, 
                message = $"Email confirmation sent to {request.Email}",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send confirmation email to {Email}", request.Email);
            
            return BadRequest(new { 
                success = false, 
                message = $"Failed to send confirmation email: {ex.Message}",
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Test password reset template
    /// </summary>
    [HttpPost("test-password-reset")]
    public async Task<IActionResult> TestPasswordReset([FromBody] PasswordResetTestRequest request)
    {
        try
        {
            await _emailService.SendPasswordResetLinkAsync(
                GetTestUser(request.Email),
                request.UserName,
                request.ResetLink ?? "https://localhost:7000/reset-password?token=testtoken"
            );

            return Ok(new { 
                success = true, 
                message = $"Password reset email sent to {request.Email}",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", request.Email);
            
            return BadRequest(new { 
                success = false, 
                message = $"Failed to send password reset email: {ex.Message}",
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Test password reset code template
    /// </summary>
    [HttpPost("test-password-reset-code")]
    public async Task<IActionResult> TestPasswordResetCode([FromBody] PasswordResetCodeTestRequest request)
    {

        try
        {
            await _emailService.SendPasswordResetCodeAsync(
                GetTestUser(request.Email),
                request.UserName,
                request.ResetCode ?? "ABC123"
            );

            return Ok(new { 
                success = true, 
                message = $"Password reset code sent to {request.Email}",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset code to {Email}", request.Email);
            
            return BadRequest(new { 
                success = false, 
                message = $"Failed to send password reset code: {ex.Message}",
                timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Get email service configuration info
    /// </summary>
    [HttpGet("config")]
    public IActionResult GetEmailConfig()
    {

        return Ok(new
        {
            service = "Apprise Email Service",
            description = "Using NApprise package for reliable email delivery with .NET Identity integration",
            features = new[]
            {
                "Multi-backend support (SMTP, Gmail, Outlook, etc.)",
                "Automatic fallback to SMTP if Apprise fails",
                "HTML email templates",
                "Email confirmation integration", 
                "Password reset functionality",
                "Password reset code support",
                "IEmailSender<User> implementation for .NET Identity"
            },
            endpoints = new
            {
                testEmail = "/api/emailtest/send-test",
                testConfirmation = "/api/emailtest/test-confirmation",
                testPasswordReset = "/api/emailtest/test-password-reset",
                testPasswordResetCode = "/api/emailtest/test-password-reset-code"
            }
        });
    }

    private User GetTestUser(string email)
    {
        return new User
        {
            Id = "test",
            Email = email,
            UserName = "test"
        };
    }
}

/// <summary>
/// Request model for testing email functionality
/// </summary>
public record TestEmailRequest(
    string To,
    string? Subject = null,
    string? Body = null
);

/// <summary>
/// Request model for testing email confirmation
/// </summary>
public record ConfirmationTestRequest(
    string Email,
    string UserName,
    string? ConfirmationLink = null
);

/// <summary>
/// Request model for testing password reset
/// </summary>
public record PasswordResetTestRequest(
    string Email,
    string UserName,
    string? ResetLink = null
);

/// <summary>
/// Request model for testing password reset code
/// </summary>
public record PasswordResetCodeTestRequest(
    string Email,
    string UserName,
    string? ResetCode = null
);