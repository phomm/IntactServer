using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Intact.BusinessLogic.Data.RedisDI
{
    public static class ApplicationBuilderExtensions
    {
        // ReSharper disable once UnusedMember.Global
        public static void StartRedisInfrastructure(this IApplicationBuilder builder, bool useInMemoryCache = false)
        {
            if (!useInMemoryCache)
            {
                builder.ApplicationServices.GetService<IStartRedisInfrastructureService>().Start(); 
            }
        }
    }
}