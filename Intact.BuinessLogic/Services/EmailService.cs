using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NApprise;

namespace Intact.BusinessLogic.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ILogger<EmailService> _logger;
    private readonly AppriseClient _appriseClient;

    public EmailService(IOptions<EmailSettings> emailSettings, IEmailTemplateService emailTemplateService, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _emailTemplateService = emailTemplateService;
        _logger = logger;
        _appriseClient = new AppriseClient();
    }

    public async Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink)
    {
        var subject = "Confirm your email - Intact Application";
        var htmlMessage = _emailTemplateService.GetEmailConfirmationTemplate(userName, confirmationLink);
        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetAsync(string email, string userName, string resetLink)
    {
        var subject = "Reset your password - Intact Application";
        var htmlMessage = _emailTemplateService.GetPasswordResetTemplate(userName, resetLink);
        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            // Use Apprise for email notifications
            if (!string.IsNullOrEmpty(_emailSettings.AppriseUrl))
            {
                await SendViaAppriseAsync(to, subject, htmlMessage);
            }
            else
            {
                // Fallback to SMTP if Apprise URL is not configured
                await SendViaSmtpAsync(to, subject, htmlMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", to);
            throw new InvalidOperationException($"Failed to send email to {to}: {ex.Message}", ex);
        }
    }

    private async Task SendViaAppriseAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            var notification = new Notification
            {
                Title = subject,
                Body = htmlMessage,
                Format = NotificationFormat.Html
            };

            // Build email URL for Apprise (e.g., "mailto://user:pass@gmail.com?to=recipient@domain.com")
            var emailUrl = BuildEmailUrl(to);
            
            var result = await _appriseClient.SendNotificationAsync(emailUrl, notification);
            
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Apprise notification failed: {string.Join(", ", result.Errors)}");
            }

            _logger.LogInformation("Email sent successfully via Apprise to {Email}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via Apprise to {Email}", to);
            throw;
        }
    }

    private string BuildEmailUrl(string to)
    {
        // Build Apprise email URL based on configuration
        if (!string.IsNullOrEmpty(_emailSettings.AppriseUrl))
        {
            // Use configured Apprise URL and append recipient
            var baseUrl = _emailSettings.AppriseUrl;
            if (baseUrl.Contains("?"))
                return $"{baseUrl}&to={to}";
            else
                return $"{baseUrl}?to={to}";
        }
        
        // Default Gmail configuration if no specific Apprise URL
        var username = Uri.EscapeDataString(_emailSettings.Username ?? "");
        var password = Uri.EscapeDataString(_emailSettings.Password ?? "");
        var smtpServer = _emailSettings.SmtpServer ?? "smtp.gmail.com";
        var smtpPort = _emailSettings.SmtpPort;
        
        return $"mailto://{username}:{password}@{smtpServer}:{smtpPort}?to={to}&from={_emailSettings.SenderEmail}";
    }

    private async Task SendViaSmtpAsync(string to, string subject, string htmlMessage)
    {
        using var client = new System.Net.Mail.SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
        client.EnableSsl = _emailSettings.EnableSsl;
        client.Credentials = new System.Net.NetworkCredential(_emailSettings.Username, _emailSettings.Password);

        var mailMessage = new System.Net.Mail.MailMessage
        {
            From = new System.Net.Mail.MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);
        await client.SendMailAsync(mailMessage);
        
        _logger.LogInformation("Email sent successfully via SMTP to {Email}", to);
    }
}