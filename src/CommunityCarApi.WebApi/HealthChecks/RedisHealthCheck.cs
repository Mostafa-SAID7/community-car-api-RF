using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace CommunityCarApi.WebApi.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer? _redis;

    public RedisHealthCheck(IConnectionMultiplexer? redis = null)
    {
        _redis = redis;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_redis == null || !_redis.IsConnected)
            {
                return HealthCheckResult.Degraded("Redis is not configured or not connected. Using in-memory cache fallback.");
            }

            var database = _redis.GetDatabase();
            await database.PingAsync();

            return HealthCheckResult.Healthy("Redis is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded(
                "Redis is unhealthy. Using in-memory cache fallback.",
                exception: ex);
        }
    }
}
