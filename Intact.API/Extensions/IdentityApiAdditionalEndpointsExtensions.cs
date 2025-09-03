using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Intact.BusinessLogic.Services;
using System.ComponentModel.DataAnnotations;

namespace Intact.API.Extensions;

public static class IdentityApiAdditionalEndpointsExtensions
{
    //https://github.com/dotnet/aspnetcore/issues/52834
    public static IEndpointRouteBuilder MapIdentityApiAdditionalEndpoints<TUser>(this IEndpointRouteBuilder endpoints)
        where TUser : class, new()
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapPost("/logout", async (SignInManager<TUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }).RequireAuthorization();

        // Email confirmation endpoint
        endpoints.MapGet("/confirmEmail", async (string userId, string code, UserManager<TUser> userManager) =>
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return Results.BadRequest("Invalid email confirmation request.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.NotFound("User not found.");
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                return Results.Ok(new { message = "Email confirmed successfully! You can now sign in." });
            }

            return Results.BadRequest(new { message = "Error confirming email.", errors = result.Errors });
        });

        // Resend email confirmation
        endpoints.MapPost("/resendEmailConfirmation", async (ResendEmailRequest request, UserManager<TUser> userManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor) =>
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Results.BadRequest("Email is required.");
            }

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return Results.Ok(new { message = "If an account with this email exists, a confirmation email has been sent." });
            }

            if (await userManager.IsEmailConfirmedAsync(user))
            {
                return Results.BadRequest("Email is already confirmed.");
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var context = httpContextAccessor.HttpContext!;
            var callbackUrl = $"{context.Request.Scheme}://{context.Request.Host}/api/confirmEmail?userId={user.Id}&code={encodedCode}";

            await emailService.SendEmailConfirmationAsync(request.Email, await userManager.GetUserNameAsync(user) ?? request.Email, callbackUrl);

            return Results.Ok(new { message = "Confirmation email sent." });
        });

        return endpoints;
    }

    public class ResendEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}