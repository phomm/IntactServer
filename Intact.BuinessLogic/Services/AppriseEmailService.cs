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
    private readonly AppriseClient _appriseClient;

    public AppriseEmailService(
        IOptions<EmailSettings> emailSettings, 
        IEmailTemplateService emailTemplateService, 
        ILogger<AppriseEmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _emailTemplateService = emailTemplateService;
        _logger = logger;
        _appriseClient = new AppriseClient();
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
            await SendViaSmtpAsync(to, subject, htmlMessage);
            
            _logger.LogInformation("Email sent successfully via SMTP to {Email}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", to);
            throw new InvalidOperationException($"Failed to send email to {to}: {ex.Message}", ex);
        }
    }

    private async Task SendViaSmtpAsync(string to, string subject, string htmlMessage)
    {
        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
        client.EnableSsl = _emailSettings.EnableSsl;
        client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);
        await client.SendMailAsync(mailMessage);
    }

    private string BuildEmailUrl(string to)
    {
        try
        {
            // If a specific Apprise URL is configured, use it
            if (!string.IsNullOrEmpty(_emailSettings.AppriseUrl))
            {
                var baseUrl = _emailSettings.AppriseUrl;
                
                // Add recipient to the URL
                if (baseUrl.Contains("?"))
                    return $"{baseUrl}&to={Uri.EscapeDataString(to)}";
                else
                    return $"{baseUrl}?to={Uri.EscapeDataString(to)}";
            }

            // Build URL from SMTP settings
            return BuildEmailUrlFromSmtpSettings(to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build email URL for {Email}", to);
            throw;
        }
    }

    private string BuildEmailUrlFromSmtpSettings(string to)
    {
        var username = Uri.EscapeDataString(_emailSettings.Username ?? "");
        var password = Uri.EscapeDataString(_emailSettings.Password ?? "");
        var smtpServer = _emailSettings.SmtpServer ?? "smtp.gmail.com";
        var smtpPort = _emailSettings.SmtpPort > 0 ? _emailSettings.SmtpPort : 587;
        var senderEmail = Uri.EscapeDataString(_emailSettings.SenderEmail ?? _emailSettings.Username ?? "");
        var senderName = Uri.EscapeDataString(_emailSettings.SenderName ?? "Intact Application");

        // Build mailto URL for Apprise
        // Format: mailto://username:password@smtp.server:port?to=recipient&from=sender&name=sendername
        var url = $"mailto://{username}:{password}@{smtpServer}:{smtpPort}";
        url += $"?to={Uri.EscapeDataString(to)}";
        url += $"&from={senderEmail}";
        
        if (!string.IsNullOrEmpty(senderName))
            url += $"&name={senderName}";

        // Add SSL if enabled
        if (_emailSettings.EnableSsl)
            url += "&secure=yes";

        return url;
    }

    /// <summary>
    /// Test the email configuration by sending a test message
    /// </summary>
    public async Task<bool> TestEmailConfigurationAsync(string testEmail = "test@example.com")
    {
        try
        {
            var testSubject = "🧪 Test Email - Intact Application";
            var testBody = "<h2>Email Configuration Test</h2><p>This is a test email to verify your email configuration is working correctly.</p><p>If you received this email, your email setup is functioning properly!</p>";
            
            await SendEmailAsync(testEmail, testSubject, testBody);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email configuration test failed");
            return false;
        }
    }
}