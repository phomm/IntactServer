using Intact.BusinessLogic.Models;

namespace Intact.BusinessLogic.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
    Task SendConfirmationLinkAsync(User user, string email, string confirmationLink);
    Task SendPasswordResetLinkAsync(User user, string email, string resetLink);
    Task SendPasswordResetCodeAsync(User user, string email, string resetCode);
}

public class EmailService : IEmailService
{
    private readonly IAppriseEmailService _appriseEmailService;
    private readonly IEmailTemplateService _emailTemplateService;

    public EmailService(IAppriseEmailService appriseEmailService, IEmailTemplateService emailTemplateService)
    {
        _appriseEmailService = appriseEmailService;
        _emailTemplateService = emailTemplateService;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        await _appriseEmailService.SendEmailAsync(to, subject, htmlMessage);
    }

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "Confirm your email - Intact Application";
        var htmlMessage = _emailTemplateService.GetEmailConfirmationTemplate(user.UserName ?? email, confirmationLink);
        await _appriseEmailService.SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var subject = "Reset your password - Intact Application";
        var htmlMessage = _emailTemplateService.GetPasswordResetTemplate(user.UserName ?? email, resetLink);
        await _appriseEmailService.SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var subject = "Your password reset code - Intact Application";
        var htmlMessage = _emailTemplateService.GetPasswordResetCodeTemplate(user.UserName ?? email, resetCode);
        await _appriseEmailService.SendEmailAsync(email, subject, htmlMessage);
    }
}