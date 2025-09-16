using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Services;
using Microsoft.EntityFrameworkCore;

namespace Intact.API.Bootstrap;

public static class InternalServicesExtensions
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {
        services.AddTransient<IUserAccessor, UserAccessor>();
        services.AddTransient<IProfileAccessor, ProfileAccessor>();
        services.AddTransient<IProtoBaseService, ProtoBaseService>();
        services.AddTransient<IMapsService, MapsService>();
        services.AddTransient<ICacheService, CacheService>();
        services.AddTransient<IProfilesService, ProfilesService>();
        services.AddTransient<IRoomsService, RoomsService>();
        // Email service registration
        services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IEmailTemplateService, EmailTemplateService>();
        
        return services;
    }

    public static IServiceCollection AddDbServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        const string assemblyName = $"{nameof(Intact)}.{nameof(API)}";

        var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>()!;

        var usePostgres = !dbSettings.UseSqlServer;
        var connectionString = usePostgres ? dbSettings.PgConnectionString : dbSettings.ConnectionString;

        services.AddDbContextFactory<AppDbContext>(GetOptions);
        services.AddDbContext<AppIdentityDbContext>(GetOptions);
        services.AddDbContext<DataSetupDbContext>(GetOptions);
        
        return services;

        void GetOptions(DbContextOptionsBuilder options) => _ = usePostgres
            ? options.UseNpgsql(connectionString, b => b.MigrationsAssembly(assemblyName))
            : options.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));
    }
}