using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Intact.BusinessLogic.Services;

public class EmailSender : IEmailSender<User>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IEmailService emailService, ILogger<EmailSender> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        try
        {
            await _emailService.SendEmailConfirmationAsync(email, user.UserName ?? email, confirmationLink);
            _logger.LogInformation("Email confirmation sent to {Email} for user {UserId}", email, user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send confirmation email to {Email} for user {UserId}", email, user.Id);
            throw;
        }
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        try
        {
            await _emailService.SendPasswordResetAsync(email, user.UserName ?? email, resetLink);
            _logger.LogInformation("Password reset email sent to {Email} for user {UserId}", email, user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email} for user {UserId}", email, user.Id);
            throw;
        }
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        try
        {
            await _emailService.SendPasswordResetCodeAsync(email, user.UserName ?? email, resetCode);
            _logger.LogInformation("Password reset code sent to {Email} for user {UserId}", email, user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset code to {Email} for user {UserId}", email, user.Id);
            throw;
        }
    }
}