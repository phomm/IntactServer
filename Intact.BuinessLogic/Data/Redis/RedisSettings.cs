namespace Intact.BusinessLogic.Data.Redis;

public class RedisSettings
{
    public string ConnectionString { get; init; }
    public int CheckAvailabilityIntervalSeconds { get; init; }
    public string Password { get; init; }
    public bool UseInMemoryCache { get; set; }
}