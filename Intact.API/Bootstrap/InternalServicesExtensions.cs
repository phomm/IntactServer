using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Services;
using Microsoft.EntityFrameworkCore;

namespace Intact.API.Bootstrap
{
    public static class InternalServicesExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddTransient<IProtoBaseService, ProtoBaseService>();
            services.AddTransient<IMapsService, MapsService>();
            services.AddTransient<ICacheService, CacheService>();
            
            return services;
        }

        public static IServiceCollection AddDbServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            const bool usePostgres = true;
            const string assemblyName = $"{nameof(Intact)}.{nameof(API)}";

            var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>()!;
            var connectionString = usePostgres ? dbSettings.PgConnectionString : dbSettings.ConnectionString;

            Action<DbContextOptionsBuilder> optionsAction = options => _ = usePostgres 
                ? options.UseNpgsql(connectionString, b => b.MigrationsAssembly(assemblyName))
                : options.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName));
            
            services.AddDbContextFactory<AppDbContext>(optionsAction);
            services.AddDbContext<AppIdentityDbContext>(optionsAction);
            
            return services;
        }
    }
}