using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Intact.BusinessLogic.Services;

namespace Intact.API.Extensions;

public static class IdentityApiAdditionalEndpointsExtensions
{
    //https://github.com/dotnet/aspnetcore/issues/52834
    public static IEndpointRouteBuilder MapIdentityApiAdditionalEndpoints<TUser>(this IEndpointRouteBuilder endpoints)
        where TUser : class, new()
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        // Enhanced email confirmation endpoint with HTML response
        endpoints.MapGet("/confirmEmail2", async (string userId, string code, UserManager<TUser> userManager, IEmailTemplateService emailTemplateService) =>
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                var errorPage = emailTemplateService.GetEmailConfirmationErrorPage();
                return Results.Content(errorPage, "text/html");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var errorPage = emailTemplateService.GetEmailConfirmationErrorPage();
                return Results.Content(errorPage, "text/html");
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                var successPage = emailTemplateService.GetEmailConfirmationSuccessPage();
                return Results.Content(successPage, "text/html");
            }

            var errorPageFinal = emailTemplateService.GetEmailConfirmationErrorPage();
            return Results.Content(errorPageFinal, "text/html");
        });

        // Auth configuration endpoint
        endpoints.MapGet("/api/auth/config", () =>
        {
            return Results.Ok(new
            {
                requireEmailConfirmation = true,
                message = "Email confirmation is enabled for new registrations"
            });
        });

        // Logout endpoint
        endpoints.MapPost("/logout", async (SignInManager<TUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok(new { message = "Logged out successfully" });
        })
        .RequireAuthorization();

        return endpoints;
    }
}