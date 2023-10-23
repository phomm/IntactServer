using Intact.BusinessLogic.Data.RedisCache;
using Intact.BusinessLogic.Models;

namespace Intact.BusinessLogic.Services
{
    public interface ICacheService
    {
        Task ClearCache(CancellationToken cancellationToken);
    }

    public class CacheService: ICacheService
    {
        private readonly IRedisCache _redisCache;

        public CacheService(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task ClearCache(CancellationToken cancellationToken)
        {
            const string protoBaseCacheSet = nameof(ProtoBase);
            const string protoBaseKey = nameof(ProtoBase);
            const string mapCacheSet = nameof(Map);
            const string mapKey = nameof(Map);

            await _redisCache.RemoveAsync(protoBaseCacheSet, protoBaseKey);
            await _redisCache.RemoveAsync(mapCacheSet, mapKey);
        }
    }
}
