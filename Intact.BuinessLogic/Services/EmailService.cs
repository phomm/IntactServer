using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Intact.BusinessLogic.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IEmailTemplateService _emailTemplateService;

    public EmailService(IOptions<EmailSettings> emailSettings, IEmailTemplateService emailTemplateService)
    {
        _emailSettings = emailSettings.Value;
        _emailTemplateService = emailTemplateService;
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
        catch (Exception ex)
        {
            // Log the exception (you might want to use ILogger here)
            throw new InvalidOperationException($"Failed to send email to {to}: {ex.Message}", ex);
        }
    }
}