using Microsoft.AspNetCore.Identity;

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

        return endpoints;
    }
}