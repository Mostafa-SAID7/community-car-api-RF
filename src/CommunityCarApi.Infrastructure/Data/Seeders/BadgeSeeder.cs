using CommunityCarApi.Domain.Entities.Community;
using CommunityCarApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunityCarApi.Infrastructure.Data.Seeders;

public class BadgeSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BadgeSeeder> _logger;

    public BadgeSeeder(ApplicationDbContext context, ILogger<BadgeSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Check if badges already exist
            if (await _context.Badges.AnyAsync())
            {
                _logger.LogInformation("Badges already exist, skipping seeding");
                return;
            }

            var badges = new[]
            {
                new Badge
                {
                    Id = Guid.NewGuid(),
                    Name = "Contributor",
                    Description = "Awarded for reaching 100 total points",
                    BadgeType = BadgeType.Contributor,
                    IconUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Badge
                {
                    Id = Guid.NewGuid(),
                    Name = "Expert",
                    Description = "Awarded for reaching 500 total points",
                    BadgeType = BadgeType.Expert,
                    IconUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Badge
                {
                    Id = Guid.NewGuid(),
                    Name = "Master",
                    Description = "Awarded for reaching 1000 total points",
                    BadgeType = BadgeType.Master,
                    IconUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Badge
                {
                    Id = Guid.NewGuid(),
                    Name = "Problem Solver",
                    Description = "Awarded for having 10 accepted answers",
                    BadgeType = BadgeType.ProblemSolver,
                    IconUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Badge
                {
                    Id = Guid.NewGuid(),
                    Name = "Great Question",
                    Description = "Awarded when your question receives 50 upvotes",
                    BadgeType = BadgeType.GreatQuestion,
                    IconUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Badge
                {
                    Id = Guid.NewGuid(),
                    Name = "Great Answer",
                    Description = "Awarded when your answer receives 50 upvotes",
                    BadgeType = BadgeType.GreatAnswer,
                    IconUrl = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _context.Badges.AddRangeAsync(badges);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully seeded {Count} badges", badges.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding badges");
        }
    }
}
