using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Intact.BusinessLogic.Data.Redis
{
    /// <summary>
    /// Redis Connection Factory
    /// </summary>
    public interface IRedisConnectionFactory
    {
        IDatabase Database { get; }

        /// <summary>
        /// Sets configuration and Starts the connection.
        /// </summary>
        void Start();
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly RedisSettings _redisSettings;
        
        public RedisConnectionFactory(IOptions<RedisSettings> redisSettings)
        {
            _redisSettings = redisSettings.Value;
        }

        /// <summary>
        /// Gets the redis connection.
        /// https://github.com/StackExchange/StackExchange.Redis/blob/master/docs/Basics.md
        /// Because the ConnectionMultiplexer does a lot, it is designed to be shared and reused between callers.
        /// You should not create a ConnectionMultiplexer per operation.
        /// It is fully thread-safe and ready for this usage.
        /// </summary>
        private ConnectionMultiplexer ConnectionMultiplexer { get; set; }

        public IDatabase Database => ConnectionMultiplexer.GetDatabase();

        /// <inheritdoc />
        public void Start()
        {
            var configurationOptions = ConfigurationOptions.Parse(_redisSettings.ConnectionString);
            configurationOptions.Password ??= _redisSettings.Password;
            ConnectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
        }
    }
}
