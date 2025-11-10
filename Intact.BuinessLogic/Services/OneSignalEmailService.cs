using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Intact.BusinessLogic.Services;

/// <summary>
/// Email service implementation using OneSignal for notifications
/// Wraps OneSignal push notifications to work with the ASP.NET Identity email sender interface
/// </summary>
public class OneSignalEmailService : IEmailSender
{
    private readonly ILogger<OneSignalEmailService> _logger;
    private readonly IOneSignalNotificationService _oneSignalService;
    private readonly IEmailSender? _fallbackEmailService;

    public OneSignalEmailService(
        ILogger<OneSignalEmailService> logger,
        IOneSignalNotificationService oneSignalService,
        IEmailSender? fallbackEmailService = null)
    {
        _logger = logger;
        _oneSignalService = oneSignalService;
        _fallbackEmailService = fallbackEmailService;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            _logger.LogInformation("Sending email via OneSignal to {Email}: {Subject}", email, subject);

            // Extract user ID from email (you might want to use a proper user lookup service)
            var userId = ExtractUserIdFromEmail(email);

            // Send through OneSignal
            var success = await _oneSignalService.SendEmailNotificationAsync(userId, subject, htmlMessage);

            if (success)
            {
                _logger.LogInformation("Email sent successfully via OneSignal to {Email}", email);
            }
            else
            {
                _logger.LogWarning("Failed to send email via OneSignal to {Email}. Attempting fallback...", email);
                await TryFallbackAsync(email, subject, htmlMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email via OneSignal to {Email}", email);
            await TryFallbackAsync(email, subject, htmlMessage);
        }
    }

    private async Task TryFallbackAsync(string email, string subject, string htmlMessage)
    {
        if (_fallbackEmailService != null)
        {
            _logger.LogInformation("Using fallback email service for {Email}", email);
            await _fallbackEmailService.SendEmailAsync(email, subject, htmlMessage);
        }
        else
        {
            _logger.LogError("No fallback email service configured. Email to {Email} was not delivered", email);
            throw new InvalidOperationException($"Failed to send email to {email} and no fallback service is available");
        }
    }

    private string ExtractUserIdFromEmail(string email)
    {
        // Simple implementation: use email as user ID
        // In production, you should look up the actual user ID from your database
        return email.Split('@')[0];
    }
}
