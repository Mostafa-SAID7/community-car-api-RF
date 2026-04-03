using CommunityCarApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunityCarApi.Infrastructure.Services.Background.Jobs;

public class CleanupExpiredTokensJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CleanupExpiredTokensJob> _logger;

    public CleanupExpiredTokensJob(IApplicationDbContext context, ILogger<CleanupExpiredTokensJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Starting cleanup of expired refresh tokens");

            var expiredTokens = await _context.RefreshTokens
                .Where(t => t.ExpiresAt < DateTime.UtcNow || t.RevokedAt != null)
                .ToListAsync();

            foreach (var token in expiredTokens)
            {
                token.IsDeleted = true;
                token.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Cleaned up {Count} expired refresh tokens", expiredTokens.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired tokens");
        }
    }
}
