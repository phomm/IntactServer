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
            .AddIdentityApiEndpoints<User>(options =>
            {
                // Password requirements
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Email confirmation requirements
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Token settings
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        return serviceCollection;
    }
}