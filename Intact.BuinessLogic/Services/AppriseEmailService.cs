using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NApprise;
using System.Net;
using System.Net.Mail;

namespace Intact.BusinessLogic.Services;

public interface IAppriseEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}

public class AppriseEmailService : IAppriseEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<AppriseEmailService> _logger;
    private readonly IAppriseStatelessClient _appriseClient;

    public AppriseEmailService(
        IOptions<EmailSettings> emailSettings, 
        ILogger<AppriseEmailService> logger,
        IAppriseStatelessClient appriseClient)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
        _appriseClient = appriseClient;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        if (_emailSettings.UseApprise)
        {
            try
            {
                await SendViaAppriseAsync(to, subject, htmlMessage);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send email via Apprise to {To}, attempting fallback", to);
                
                if (!_emailSettings.FallbackToSmtp)
                {
                    throw;
                }
            }
        }

        // Fallback to SMTP or direct SMTP if Apprise is disabled
        if (_emailSettings.FallbackToSmtp || !_emailSettings.UseApprise)
        {
            await SendViaSmtpAsync(to, subject, htmlMessage);
        }
    }

    private async Task SendViaAppriseAsync(string to, string subject, string htmlMessage)
    {
        var appriseUrl = BuildAppriseEmailUrl(to);
        var urls = new List<string> { appriseUrl };
        if (_emailSettings.AppriseUrls?.Length > 0)
        {
            urls.AddRange(_emailSettings.AppriseUrls);
        }

        var notification = new StatelessNotificationRequest
        {
            Title = subject,
            Body = htmlMessage,
            Type = NotificationType.Info,
            Format = NotificationFormat.Html,
            Urls = urls
        };

        try
        {
            await _appriseClient.SendNotificationAsync(notification);
            _logger.LogDebug("Sending email via Apprise to {To} using URLs: {Urls}", to, string.Join(", ", urls));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log Apprise email sending attempt to {To}", to);
        }
    }

    private async Task SendViaSmtpAsync(string to, string subject, string htmlMessage)
    {
        try
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
            
            _logger.LogInformation("Email sent successfully via SMTP fallback to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via SMTP to {To}", to);
            throw new InvalidOperationException($"Failed to send email to {to}: {ex.Message}", ex);
        }
    }

    private string BuildAppriseEmailUrl(string to)
    {
        // Build mailto URL for Apprise
        // Format: mailto://user:password@smtp.server:port/?from=sender@domain.com&to=recipient@domain.com&name=SenderName
        
        var url = $"mailto://{Uri.EscapeDataString(_emailSettings.Username)}:{Uri.EscapeDataString(_emailSettings.Password)}@{_emailSettings.SmtpServer}:{_emailSettings.SmtpPort}";
        
        var parameters = new List<string>
        {
            $"from={Uri.EscapeDataString(_emailSettings.SenderEmail)}",
            $"to={Uri.EscapeDataString(to)}",
            $"name={Uri.EscapeDataString(_emailSettings.SenderName)}"
        };

        if (_emailSettings.EnableSsl)
        {
            parameters.Add("secure=yes");
        }

        url += "/?" + string.Join("&", parameters);
        
        _logger.LogDebug("Built Apprise email URL for {To}", to);
        return url;
    }
}