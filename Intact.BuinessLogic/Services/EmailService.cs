using Microsoft.Extensions.Logging;

namespace Intact.BusinessLogic.Services;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink);
    Task SendPasswordResetAsync(string email, string userName, string resetLink);
    Task SendPasswordResetCodeAsync(string email, string userName, string resetCode);
}

public class EmailService : IEmailService
{
    private readonly IAppriseEmailService _appriseEmailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IAppriseEmailService appriseEmailService, IEmailTemplateService emailTemplateService, ILogger<EmailService> logger)
    {
        _appriseEmailService = appriseEmailService;
        _emailTemplateService = emailTemplateService;
        _logger = logger;
    }

    public async Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink)
    {
        try
        {
            var subject = "Confirm your email - Intact Application";
            var htmlMessage = _emailTemplateService.GetEmailConfirmationTemplate(userName, confirmationLink);
            await _appriseEmailService.SendEmailAsync(email, subject, htmlMessage);
            _logger.LogInformation("Email confirmation sent successfully to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email confirmation to {Email}", email);
            throw;
        }
    }

    public async Task SendPasswordResetAsync(string email, string userName, string resetLink)
    {
        try
        {
            var subject = "Reset your password - Intact Application";
            var htmlMessage = _emailTemplateService.GetPasswordResetTemplate(userName, resetLink);
            await _appriseEmailService.SendEmailAsync(email, subject, htmlMessage);
            _logger.LogInformation("Password reset email sent successfully to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
            throw;
        }
    }

    public async Task SendPasswordResetCodeAsync(string email, string userName, string resetCode)
    {
        try
        {
            var subject = "Your password reset code - Intact Application";
            var htmlMessage = _emailTemplateService.GetPasswordResetCodeTemplate(userName, resetCode);
            await _appriseEmailService.SendEmailAsync(email, subject, htmlMessage);
            _logger.LogInformation("Password reset code sent successfully to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset code to {Email}", email);
            throw;
        }
    }
}