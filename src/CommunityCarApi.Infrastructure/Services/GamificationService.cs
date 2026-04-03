using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Domain.Entities.Community;
using CommunityCarApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Infrastructure.Services;

public class GamificationService : IGamificationService
{
    private readonly IApplicationDbContext _context;

    public GamificationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AwardPointsForQuestionAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 5;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints += points;
        reputation.QuestionsAsked++;
        
        UpdateLevelAndRank(reputation);
        await CheckAndAwardBadgesAsync(userId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return points;
    }

    public async Task<int> AwardPointsForAnswerAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 10;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints += points;
        reputation.AnswersProvided++;
        
        UpdateLevelAndRank(reputation);
        await CheckAndAwardBadgesAsync(userId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return points;
    }

    public async Task<int> AwardPointsForQuestionUpvoteAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 5;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints += points;
        
        UpdateLevelAndRank(reputation);
        await CheckAndAwardBadgesAsync(userId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return points;
    }

    public async Task<int> DeductPointsForQuestionDownvoteAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 2;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints = Math.Max(0, reputation.TotalPoints - points);
        
        UpdateLevelAndRank(reputation);
        await _context.SaveChangesAsync(cancellationToken);
        
        return -points;
    }

    public async Task<int> AwardPointsForAnswerUpvoteAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 10;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints += points;
        
        UpdateLevelAndRank(reputation);
        await CheckAndAwardBadgesAsync(userId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return points;
    }

    public async Task<int> DeductPointsForAnswerDownvoteAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 5;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints = Math.Max(0, reputation.TotalPoints - points);
        
        UpdateLevelAndRank(reputation);
        await _context.SaveChangesAsync(cancellationToken);
        
        return -points;
    }

    public async Task<int> AwardPointsForAcceptedAnswerAsync(string userId, CancellationToken cancellationToken = default)
    {
        const int points = 25;
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        
        reputation.TotalPoints += points;
        reputation.AcceptedAnswers++;
        
        UpdateLevelAndRank(reputation);
        await CheckAndAwardBadgesAsync(userId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return points;
    }

    public async Task UpdateLevelAndRankAsync(string userId, CancellationToken cancellationToken = default)
    {
        var reputation = await GetOrCreateReputationAsync(userId, cancellationToken);
        UpdateLevelAndRank(reputation);
    }

    private void UpdateLevelAndRank(UserReputation reputation)
    {
        var (level, rank) = CalculateLevelAndRank(reputation.TotalPoints);
        
        reputation.Level = level;
        reputation.Rank = rank;
    }

    public async Task CheckAndAwardBadgesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var reputation = await _context.UserReputations
            .Include(r => r.Badges)
            .ThenInclude(ub => ub.Badge)
            .FirstOrDefaultAsync(r => r.UserId == userId, cancellationToken);

        if (reputation == null)
            return;

        var allBadges = await _context.Badges.ToListAsync(cancellationToken);
        var userBadgeTypes = reputation.Badges.Select(ub => ub.Badge.BadgeType).ToHashSet();

        // Check Contributor badge (100 points)
        if (reputation.TotalPoints >= 100 && !userBadgeTypes.Contains(BadgeType.Contributor))
        {
            await AwardBadgeAsync(userId, BadgeType.Contributor, allBadges, cancellationToken);
        }

        // Check Expert badge (500 points)
        if (reputation.TotalPoints >= 500 && !userBadgeTypes.Contains(BadgeType.Expert))
        {
            await AwardBadgeAsync(userId, BadgeType.Expert, allBadges, cancellationToken);
        }

        // Check Master badge (1000 points)
        if (reputation.TotalPoints >= 1000 && !userBadgeTypes.Contains(BadgeType.Master))
        {
            await AwardBadgeAsync(userId, BadgeType.Master, allBadges, cancellationToken);
        }

        // Check Problem Solver badge (10 accepted answers)
        if (reputation.AcceptedAnswers >= 10 && !userBadgeTypes.Contains(BadgeType.ProblemSolver))
        {
            await AwardBadgeAsync(userId, BadgeType.ProblemSolver, allBadges, cancellationToken);
        }

        // Check Great Question badge (question with 50 upvotes)
        if (!userBadgeTypes.Contains(BadgeType.GreatQuestion))
        {
            var hasGreatQuestion = await _context.Questions
                .AnyAsync(q => q.UserId == userId && q.VoteCount >= 50, cancellationToken);
            
            if (hasGreatQuestion)
            {
                await AwardBadgeAsync(userId, BadgeType.GreatQuestion, allBadges, cancellationToken);
            }
        }

        // Check Great Answer badge (answer with 50 upvotes)
        if (!userBadgeTypes.Contains(BadgeType.GreatAnswer))
        {
            var hasGreatAnswer = await _context.Answers
                .AnyAsync(a => a.UserId == userId && a.VoteCount >= 50, cancellationToken);
            
            if (hasGreatAnswer)
            {
                await AwardBadgeAsync(userId, BadgeType.GreatAnswer, allBadges, cancellationToken);
            }
        }
    }

    public async Task<UserReputation> GetOrCreateReputationAsync(string userId, CancellationToken cancellationToken = default)
    {
        var reputation = await _context.UserReputations
            .FirstOrDefaultAsync(r => r.UserId == userId, cancellationToken);

        if (reputation == null)
        {
            reputation = new UserReputation
            {
                UserId = userId,
                TotalPoints = 0,
                Level = 1,
                Rank = "Beginner",
                QuestionsAsked = 0,
                AnswersProvided = 0,
                AcceptedAnswers = 0
            };

            _context.UserReputations.Add(reputation);
        }

        return reputation;
    }

    private static (int Level, string Rank) CalculateLevelAndRank(int totalPoints)
    {
        return totalPoints switch
        {
            >= 2500 => (5, "Legend"),
            >= 1000 => (4, "Master"),
            >= 500 => (3, "Expert"),
            >= 100 => (2, "Contributor"),
            _ => (1, "Beginner")
        };
    }

    private Task AwardBadgeAsync(string userId, BadgeType badgeType, List<Badge> allBadges, CancellationToken cancellationToken)
    {
        var badge = allBadges.FirstOrDefault(b => b.BadgeType == badgeType);
        
        if (badge == null)
            return Task.CompletedTask;

        var userBadge = new UserBadge
        {
            UserId = userId,
            BadgeId = badge.Id,
            EarnedAt = DateTime.UtcNow
        };

        _context.UserBadges.Add(userBadge);
        return Task.CompletedTask;
    }
}
