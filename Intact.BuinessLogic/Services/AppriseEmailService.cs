using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NApprise;

namespace Intact.BusinessLogic.Services;

/// <summary>
/// Apprise-based email service implementation for reliable email delivery
/// Supports multiple notification backends including SMTP, Gmail, Outlook, and more
/// </summary>
public class AppriseEmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ILogger<AppriseEmailService> _logger;
    private readonly IAppriseStatelessClient _appriseClient;

    public AppriseEmailService(
        IOptions<EmailSettings> emailSettings,
        IEmailTemplateService emailTemplateService,
        ILogger<AppriseEmailService> logger,
        IAppriseStatelessClient appriseClient)
    {
        _emailSettings = emailSettings.Value;
        _emailTemplateService = emailTemplateService;
        _logger = logger;
        _appriseClient = appriseClient;
    }

    public async Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink)
    {
        var subject = "✅ Confirm your email - Intact Application";
        var htmlMessage = _emailTemplateService.GetEmailConfirmationTemplate(userName, confirmationLink);
        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetAsync(string email, string userName, string resetLink)
    {
        var subject = "🔒 Reset your password - Intact Application";
        var htmlMessage = _emailTemplateService.GetPasswordResetTemplate(userName, resetLink);
        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            _logger.LogInformation("Sending email to {Email} with subject: {Subject}", to, subject);

            // Use basic SMTP for now - Apprise can be re-added later if needed
            await SendAsync(to, subject, htmlMessage);

            _logger.LogInformation("Email sent successfully via SMTP to {Email}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", to);
            throw new InvalidOperationException($"Failed to send email to {to}: {ex.Message}", ex);
        }
    }

    private async Task SendAsync(string to, string subject, string htmlMessage)
    {
        // Construct the payload for Apprise
        var payload = new StatelessNotificationRequest
        {
            Urls = [to],
            Title = subject,
            Body = htmlMessage,
            Type = NotificationType.Info,
            Format = NotificationFormat.Html
        };

        // Send the notification via Apprise
        try
        {
            await _appriseClient.SendNotificationAsync(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification via Apprise");
            throw;
        }
    }
}