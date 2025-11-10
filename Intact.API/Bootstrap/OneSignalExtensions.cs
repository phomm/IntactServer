using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Intact.API.Bootstrap;

public static class OneSignalExtensions
{
    /// <summary>
    /// Add OneSignal notification services to the DI container
    /// </summary>
    public static IServiceCollection AddOneSignalServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register OneSignal settings
        services.Configure<OneSignalSettings>(configuration.GetSection("OneSignalSettings"));

        // Register OneSignal notification service
        services.AddSingleton<IOneSignalNotificationService, OneSignalNotificationService>();

        return services;
    }

    /// <summary>
    /// Add OneSignal as the email service provider with optional fallback
    /// </summary>
    public static IServiceCollection AddOneSignalEmailService(this IServiceCollection services, bool useFallback = true)
    {
        if (useFallback)
        {
            // Register the original email service as fallback
            services.AddScoped<EmailService>();
            
            // Register OneSignalEmailService with fallback
            services.AddScoped<IEmailSender>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<OneSignalEmailService>>();
                var oneSignalService = provider.GetRequiredService<IOneSignalNotificationService>();
                var fallbackService = provider.GetRequiredService<EmailService>();
                
                return new OneSignalEmailService(logger, oneSignalService, fallbackService);
            });
        }
        else
        {
            // Register OneSignalEmailService without fallback
            services.AddScoped<IEmailSender, OneSignalEmailService>();
        }

        return services;
    }

    /// <summary>
    /// Add OneSignal notification services with email integration
    /// </summary>
    public static IServiceCollection AddOneSignalWithEmail(this IServiceCollection services, IConfiguration configuration, bool useFallback = true)
    {
        services.AddOneSignalServices(configuration);
        services.AddOneSignalEmailService(useFallback);

        return services;
    }
}
