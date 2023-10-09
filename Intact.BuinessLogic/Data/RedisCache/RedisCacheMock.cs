using Microsoft.Extensions.Caching.Memory;

namespace Intact.BusinessLogic.Data.RedisCache;

public class RedisCacheMock : IRedisCache
{
    private readonly IMemoryCache _memoryCache;

    public RedisCacheMock(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<bool> AddAsync<T>(string cacheSet, string key, T value)
    {
        _memoryCache.Set(key, value);
        return true;
    }

    public async Task<T> GetAsync<T>(string cacheSet, string key) where T : class
    {
        return _memoryCache.Get<T>(key);
    }

    public async Task RemoveAsync(string cacheSet, string key)
    {
        _memoryCache.Remove(key);
    }

    public async Task<bool> ExistsAsync(string cacheSet, string key)
    {
        return _memoryCache.TryGetValue(key, out var data);
    }

    public async Task<bool> AddStringAsync<T>(string cacheSet, string key, T value, TimeSpan expiration)
    {
        _memoryCache.Set(key, value);
        return true;
    }

    public async Task<T> GetStringAsync<T>(string cacheSet, string key) where T : class
    {
        return _memoryCache.Get<T>(key);
    }

    public async Task RemoveStringAsync(string cacheSet, string key)
    {
        _memoryCache.Remove(key);
    }

    public async Task<bool> StringExistsAsync(string cacheSet, string key)
    {
        return _memoryCache.TryGetValue(key, out var data);
    }
}