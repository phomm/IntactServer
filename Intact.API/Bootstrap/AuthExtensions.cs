using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace Intact.API.Bootstrap;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        serviceCollection.AddAuthorizationBuilder();
        serviceCollection.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddApiEndpoints();

        return serviceCollection;
    }
}