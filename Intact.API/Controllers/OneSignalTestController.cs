using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Services;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OneSignalTestController : ControllerBase
{
    private readonly ILogger<OneSignalTestController> _logger;
    private readonly IOneSignalNotificationService _oneSignalService;
    private readonly OneSignalSettings _settings;

    public OneSignalTestController(
        ILogger<OneSignalTestController> logger,
        IOneSignalNotificationService oneSignalService,
        IOptions<OneSignalSettings> settings)
    {
        _logger = logger;
        _oneSignalService = oneSignalService;
        _settings = settings.Value;
    }

    /// <summary>
    /// Get OneSignal configuration (without sensitive data)
    /// </summary>
    [HttpGet("config")]
    public IActionResult GetConfig()
    {
        return Ok(new
        {
            _settings.Enabled,
            AppId = !string.IsNullOrEmpty(_settings.AppId) ? "***configured***" : null,
            RestApiKey = !string.IsNullOrEmpty(_settings.RestApiKey) ? "***configured***" : null,
            UserAuthKey = !string.IsNullOrEmpty(_settings.UserAuthKey) ? "***configured***" : null,
            _settings.DefaultIconUrl,
            _settings.DefaultBadgeUrl,
            _settings.DefaultSound,
            _settings.TimeoutSeconds
        });
    }

    /// <summary>
    /// Send a test notification to a specific user
    /// </summary>
    [HttpPost("send-to-user")]
    public async Task<IActionResult> SendToUser([FromBody] SendToUserRequest request)
    {
        if (!_settings.Enabled)
        {
            return BadRequest(new { error = "OneSignal is not enabled. Check your configuration." });
        }

        if (string.IsNullOrEmpty(request.UserId))
        {
            return BadRequest(new { error = "UserId is required" });
        }

        _logger.LogInformation("Test notification request for user {UserId}", request.UserId);

        var success = await _oneSignalService.SendToUserAsync(
            request.UserId,
            request.Title ?? "Test Notification",
            request.Message ?? "This is a test notification from Intact Server",
            request.Data
        );

        if (success)
        {
            return Ok(new { message = "Notification sent successfully", userId = request.UserId });
        }

        return StatusCode(500, new { error = "Failed to send notification" });
    }

    /// <summary>
    /// Send a broadcast notification to all users
    /// </summary>
    [HttpPost("broadcast")]
    public async Task<IActionResult> Broadcast([FromBody] BroadcastRequest request)
    {
        if (!_settings.Enabled)
        {
            return BadRequest(new { error = "OneSignal is not enabled. Check your configuration." });
        }

        _logger.LogInformation("Broadcast notification request");

        var success = await _oneSignalService.SendBroadcastAsync(
            request.Title ?? "Broadcast",
            request.Message ?? "This is a broadcast message",
            request.Data
        );

        if (success)
        {
            return Ok(new { message = "Broadcast sent successfully" });
        }

        return StatusCode(500, new { error = "Failed to send broadcast" });
    }

    /// <summary>
    /// Register a device for push notifications
    /// </summary>
    [HttpPost("register-device")]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceRequest request)
    {
        if (!_settings.Enabled)
        {
            return BadRequest(new { error = "OneSignal is not enabled. Check your configuration." });
        }

        if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.DeviceToken))
        {
            return BadRequest(new { error = "UserId and DeviceToken are required" });
        }

        _logger.LogInformation("Device registration request for user {UserId}", request.UserId);

        var success = await _oneSignalService.RegisterUserDeviceAsync(
            request.UserId,
            request.DeviceToken,
            request.DeviceType ?? "Android"
        );

        if (success)
        {
            return Ok(new { message = "Device registered successfully", userId = request.UserId });
        }

        return StatusCode(500, new { error = "Failed to register device" });
    }

    /// <summary>
    /// Update user tags for segmentation
    /// </summary>
    [HttpPost("update-tags")]
    public async Task<IActionResult> UpdateTags([FromBody] UpdateTagsRequest request)
    {
        if (!_settings.Enabled)
        {
            return BadRequest(new { error = "OneSignal is not enabled. Check your configuration." });
        }

        if (string.IsNullOrEmpty(request.UserId) || request.Tags == null || !request.Tags.Any())
        {
            return BadRequest(new { error = "UserId and Tags are required" });
        }

        _logger.LogInformation("Update tags request for user {UserId}", request.UserId);

        var success = await _oneSignalService.UpdateUserTagsAsync(request.UserId, request.Tags);

        if (success)
        {
            return Ok(new { message = "Tags updated successfully", userId = request.UserId });
        }

        return StatusCode(500, new { error = "Failed to update tags" });
    }

    /// <summary>
    /// Send an email-style notification
    /// </summary>
    [HttpPost("send-email-notification")]
    public async Task<IActionResult> SendEmailNotification([FromBody] EmailNotificationRequest request)
    {
        if (!_settings.Enabled)
        {
            return BadRequest(new { error = "OneSignal is not enabled. Check your configuration." });
        }

        if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Subject))
        {
            return BadRequest(new { error = "UserId and Subject are required" });
        }

        _logger.LogInformation("Email notification request for user {UserId}", request.UserId);

        var success = await _oneSignalService.SendEmailNotificationAsync(
            request.UserId,
            request.Subject,
            request.HtmlBody ?? "This is a test email notification"
        );

        if (success)
        {
            return Ok(new { message = "Email notification sent successfully", userId = request.UserId });
        }

        return StatusCode(500, new { error = "Failed to send email notification" });
    }
}

// Request models
public class SendToUserRequest
{
    public string UserId { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}

public class BroadcastRequest
{
    public string? Title { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}

public class RegisterDeviceRequest
{
    public string UserId { get; set; } = string.Empty;
    public string DeviceToken { get; set; } = string.Empty;
    public string? DeviceType { get; set; }
}

public class UpdateTagsRequest
{
    public string UserId { get; set; } = string.Empty;
    public Dictionary<string, string> Tags { get; set; } = new();
}

public class EmailNotificationRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? HtmlBody { get; set; }
}
