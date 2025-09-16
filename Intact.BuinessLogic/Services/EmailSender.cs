using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace Intact.BusinessLogic.Services;

public class EmailSender : IEmailSender<User>
{
    private readonly IEmailService _emailService;

    public EmailSender(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        await _emailService.SendEmailAsync(to, subject, htmlMessage);
    }

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        await _emailService.SendConfirmationLinkAsync(user, email, confirmationLink);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        await _emailService.SendPasswordResetLinkAsync(user, email, resetLink);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        await _emailService.SendPasswordResetCodeAsync(user, email, resetCode);
    }
}