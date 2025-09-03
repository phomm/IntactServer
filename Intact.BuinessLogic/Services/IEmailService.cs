namespace Intact.BusinessLogic.Services;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string email, string userName, string confirmationLink);
    Task SendPasswordResetAsync(string email, string userName, string resetLink);
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}