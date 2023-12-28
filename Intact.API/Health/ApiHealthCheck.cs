using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Intact.API.Health;

public class ApiHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(HealthCheckResult.Healthy());
    }
}