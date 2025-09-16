using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NApprise;

namespace Intact.BusinessLogic.Services;

public interface IAppriseEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}

public class AppriseEmailService : IAppriseEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<AppriseEmailService> _logger;
    private readonly AppriseClient _appriseClient;

    public AppriseEmailService(IOptions<EmailSettings> emailSettings, ILogger<AppriseEmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
        _appriseClient = new AppriseClient();
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            // Build Apprise email URL (mailto:// protocol)
            var appriseUrl = BuildAppriseEmailUrl(to);
            
            var notification = new NotificationRequest
            {
                Title = subject,
                Body = htmlMessage,
                NotifyType = NotifyType.Info,
                BodyFormat = NotifyFormat.Html
            };

            await _appriseClient.NotifyAsync(appriseUrl, notification);
            
            _logger.LogInformation("Email sent successfully via Apprise to {To} with subject '{Subject}'", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via Apprise to {To} with subject '{Subject}'", to, subject);
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