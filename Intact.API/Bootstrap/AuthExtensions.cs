using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Intact.API.Bootstrap;

public static class AuthExtensions
{
    public static IServiceCollection CustomizeAuthorization(this IServiceCollection serviceCollection)
    {
        var defaultPolicy =
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

        serviceCollection.AddAuthorizationBuilder()
            .AddDefaultPolicy(nameof(defaultPolicy), defaultPolicy)
            .AddPolicy(Policies.Admin, builder => builder
                    .RequireRole(Policies.Admin)
                    .Combine(defaultPolicy));
        
        return serviceCollection;
    }

    public static IServiceCollection CustomizeAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication();
        serviceCollection
            .AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        return serviceCollection;
    }
}