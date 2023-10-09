using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Timer = System.Timers.Timer;

namespace Intact.BusinessLogic.Data.Redis
{
    /// <summary>
    /// For failover availability pings the Redis storage at intervals
    /// and when connection fails calls Reconnect.
    /// 
    /// This is the entry point to start Redis infrastructure.
    /// More info: https://redis.io/topics/sentinel
    /// </summary>
    public interface IRedisPingService
    {
        /// <summary>
        /// Starts to ping Redis storage at intervals.
        /// </summary>
        void Start();
    }
    /// <inheritdoc />
    public class RedisPingService : IRedisPingService
    {
        private readonly ILogger<RedisPingService> _logger;
        private readonly IRedisConnectionFactory _redisConnectionFactory;
        private readonly RedisSettings _redisSettings;
        private IConnectionMultiplexer _redisConnection;

        private const string RedisKey = nameof(RedisPingService);
        private const int MillisecondsInSecond = 1000;

        public RedisPingService(IRedisConnectionFactory redisConnectionFactory, IOptions<RedisSettings> redisSettings, ILogger<RedisPingService> logger)
        {
            _logger = logger;
            _redisConnectionFactory = redisConnectionFactory;
            _redisSettings = redisSettings.Value;
        }

        /// <inheritdoc />
        public void Start()
        {
            _redisConnectionFactory.Start();
            _redisConnection = _redisConnectionFactory.RedisConnection;
            var timer = new Timer(_redisSettings.CheckAvailabilityIntervalSeconds * MillisecondsInSecond) { Enabled = true };
            timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var database = _redisConnection.GetDatabase();

            if (_redisConnection.IsConnected)
                return;

            _logger.LogError("Redis Disconnected. Trying to ping again...");

            // If not connected, make a call to let exception bubble
            database.Ping(CommandFlags.DemandMaster);

            return; // if it didn't throw, it's back up
        }
    }
}
