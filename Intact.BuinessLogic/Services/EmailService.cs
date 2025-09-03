using Intact.BusinessLogic.Data.Config;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Intact.BusinessLogic.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink)
    {
        var subject = "Confirm your email - Intact Application";
        var htmlMessage = EmailTemplates.GetEmailConfirmationTemplate(userName, confirmationLink);
        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetAsync(string email, string userName, string resetLink)
    {
        var subject = "Reset your password - Intact Application";
        var htmlMessage = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #e74c3c; text-align: center;'>Password Reset Request</h2>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset your password for your Intact Application account. Click the link below to reset your password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #e74c3c; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Reset Password
                        </a>
                    </div>
                    <p>If you cannot click the button above, copy and paste the following link into your browser:</p>
                    <p style='word-break: break-all; color: #7f8c8d;'>{resetLink}</p>
                    <p>If you did not request this password reset, please ignore this email. Your password will not be changed.</p>
                    <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 30px 0;'>
                    <p style='color: #7f8c8d; font-size: 12px; text-align: center;'>
                        This is an automated message from Intact Application. Please do not reply to this email.
                    </p>
                </div>
            </body>
            </html>";

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