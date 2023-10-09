namespace Intact.BusinessLogic.Data.RedisCache;

public interface IRedisCache
{
    Task<bool> AddAsync<T>(string cacheSet, string key, T value);
    Task<T> GetAsync<T>(string cacheSet, string key) where T : class;
    Task RemoveAsync(string cacheSet, string key);
    Task<bool> ExistsAsync(string cacheSet, string key);

    Task<bool> AddStringAsync<T>(string cacheSet, string key, T value, TimeSpan expiration);
    Task<T> GetStringAsync<T>(string cacheSet, string key) where T : class;
    Task RemoveStringAsync(string cacheSet, string key);
    Task<bool> StringExistsAsync(string cacheSet, string key);
}