using System.Text.Json;
using Intact.BusinessLogic.Data.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Intact.BusinessLogic.Data.RedisCache;

public class RedisCache : IRedisCache
{
    private readonly Lazy<IDatabase> _database;
    private readonly ILogger<RedisCache> _logger;

    public RedisCache(IRedisConnectionFactory redisConnectionFactory, ILogger<RedisCache> logger)
    {
        _logger = logger;
        _database = new Lazy<IDatabase>(() => redisConnectionFactory.RedisConnection.GetDatabase());
    }

    public async Task<bool> AddAsync<T>(string cacheSet, string key, T value)
    {
        var serialized = JsonSerializer.Serialize(value);

        _logger.LogDebug("AddAsync to cacheSet={cacheSet} with key={key}, value={value}", cacheSet, key, serialized);

        return await _database.Value.HashSetAsync(cacheSet, key, serialized).ConfigureAwait(false);
          
    }

    public async Task<T?> GetAsync<T>(string cacheSet, string key) where T : class
    {
        var data =  await _database.Value.HashGetAsync(cacheSet, key).ConfigureAwait(false);

        _logger.LogDebug("GetAsync from cacheSet={cacheSet} with key={key}. Result={@result}", cacheSet, key, data);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>(data!);
    }

    public async Task RemoveAsync(string cacheSet, string key)
    {
        _logger.LogDebug("RemoveAsync from cacheSet={cacheSet} with key={key}.", cacheSet, key);

        await _database.Value.HashDeleteAsync(cacheSet, key).ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync(string cacheSet, string key)
    {
        return  await _database.Value.HashExistsAsync(cacheSet, key).ConfigureAwait(false);
    }

    public async Task<bool> AddStringAsync<T>(string cacheSet, string key, T value, TimeSpan expiration)
    {
        var serialized = JsonSerializer.Serialize(value);

        _logger.LogDebug("AddStringAsync to {cacheSet}:{key} with value={value}", cacheSet, key, serialized);
            
        return await _database.Value.StringSetAsync(CreateStringKey(cacheSet, key), serialized, expiration).ConfigureAwait(false);
    }

    public async Task<T?> GetStringAsync<T>(string cacheSet, string key) where T : class
    {
        var data = await _database.Value.StringGetAsync(CreateStringKey(cacheSet, key)).ConfigureAwait(false);

        if (data.IsNullOrEmpty)
            return null;

        var result = JsonSerializer.Deserialize<T>(data!);

        _logger.LogDebug("GetStringAsync from {cacheSet}:{key}. Returns {@result}", cacheSet, key, result);

        return result;
    }

    public async Task RemoveStringAsync(string cacheSet, string key)
    {
        _logger.LogDebug("RemoveStringAsync from {cacheSet}:{key}.", cacheSet, key);

        await _database.Value.StringGetDeleteAsync(CreateStringKey(cacheSet, key)).ConfigureAwait(false);
    }

    public async Task<bool> StringExistsAsync(string cacheSet, string key)
    {
        var data = await _database.Value.StringGetAsync(CreateStringKey(cacheSet, key)).ConfigureAwait(false);

        return !data.IsNull;
    }

    private static string CreateStringKey(string cacheSet, string key)
    {
        return $"{cacheSet}:{key}";
    }
}