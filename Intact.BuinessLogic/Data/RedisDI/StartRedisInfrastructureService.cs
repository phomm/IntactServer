using Intact.BusinessLogic.Data.Redis;

namespace Intact.BusinessLogic.Data.RedisDI;

public interface IStartRedisInfrastructureService
{
    void Start();
}

public class StartRedisInfrastructureService : IStartRedisInfrastructureService
{
    private readonly IRedisPingService _redisPingService;

    public StartRedisInfrastructureService(IRedisPingService redisPingService)
    {
        _redisPingService = redisPingService;
    }

    public void Start()
    {
        _redisPingService.Start();
    }
}