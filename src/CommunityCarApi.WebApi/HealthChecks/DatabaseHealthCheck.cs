using CommunityCarApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CommunityCarApi.WebApi.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly ApplicationDbContext _context;

    public DatabaseHealthCheck(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to connect to the database
            await _context.Database.CanConnectAsync(cancellationToken);
            
            // Check if there are any pending migrations
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync(cancellationToken);
            
            if (pendingMigrations.Any())
            {
                return HealthCheckResult.Degraded(
                    $"Database has {pendingMigrations.Count()} pending migrations",
                    data: new Dictionary<string, object>
                    {
                        { "pendingMigrations", string.Join(", ", pendingMigrations) }
                    });
            }

            return HealthCheckResult.Healthy("Database is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Database is unhealthy",
                exception: ex);
        }
    }
}
