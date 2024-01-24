using Microsoft.Extensions.DependencyInjection;
using Intact.BusinessLogic.Data.Redis;
using Intact.BusinessLogic.Data.RedisCache;
using Microsoft.Extensions.Caching.Memory;

namespace Intact.BusinessLogic.Data.RedisDI
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterRedisServices(this IServiceCollection services, bool useInMemoryCache = false)
        {
            if (useInMemoryCache)
            {
                services.AddSingleton<IMemoryCache, MemoryCache>();
                services.AddSingleton<IRedisCache, RedisCacheMock>();
            }
            else
            {
                services.AddSingleton<IRedisCache, RedisCache.RedisCache>();
                services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
            }
        }
    }
}