using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Intact.BusinessLogic.Services;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink);
    Task SendPasswordResetAsync(string email, string userName, string resetLink);
}

public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplateService _emailTemplateService;

    public EmailService(IEmailSender emailSender, IEmailTemplateService emailTemplateService)
    {
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
    }

    public async Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink)
    {
        var subject = "Confirm your email - Intact Application";
        var htmlMessage = _emailTemplateService.GetEmailConfirmationTemplate(userName, confirmationLink);
        await _emailSender.SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetAsync(string email, string userName, string resetLink)
    {
        var subject = "Reset your password - Intact Application";
        var htmlMessage = _emailTemplateService.GetPasswordResetTemplate(userName, resetLink);
        await _emailSender.SendEmailAsync(email, subject, htmlMessage);
    }
}