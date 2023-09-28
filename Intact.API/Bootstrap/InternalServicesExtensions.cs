using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Services;
using Microsoft.EntityFrameworkCore;

namespace Intact.API.Bootstrap
{
    public static class InternalServicesExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddTransient<IProtoBaseService, ProtoBaseService>();

            var connectionString = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>().ConnectionString;
            
            services.AddDbContextFactory<IntactDbContext>(
                options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly($"{nameof(Intact)}.{nameof(API)}")));

            return services;
        }
    }
}