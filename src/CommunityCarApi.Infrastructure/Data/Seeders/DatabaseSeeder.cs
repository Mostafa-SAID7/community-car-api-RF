using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommunityCarApi.Infrastructure.Data.Seeders;

public class DatabaseSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            // Seed roles
            var roleSeeder = ActivatorUtilities.CreateInstance<RoleSeeder>(services);
            await roleSeeder.SeedAsync();

            // Seed admin user
            var adminSeeder = ActivatorUtilities.CreateInstance<AdminUserSeeder>(services);
            await adminSeeder.SeedAsync();

            // Seed badges
            var badgeSeeder = ActivatorUtilities.CreateInstance<BadgeSeeder>(services);
            await badgeSeeder.SeedAsync();

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database seeding");
        }
    }
}
