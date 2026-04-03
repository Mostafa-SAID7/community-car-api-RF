using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetUserReputationQueryHandler : IRequestHandler<GetUserReputationQuery, Result<UserReputationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGamificationService _gamificationService;

    public GetUserReputationQueryHandler(
        IApplicationDbContext context, 
        ICurrentUserService currentUserService,
        IGamificationService gamificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _gamificationService = gamificationService;
    }

    public async Task<Result<UserReputationDto>> Handle(GetUserReputationQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId;
        
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Result<UserReputationDto>.Failure("User is not authenticated");
        }

        // Get or create user reputation
        var reputation = await _gamificationService.GetOrCreateReputationAsync(currentUserId, cancellationToken);

        // Query user badges with badge details
        var userBadges = await _context.UserBadges
            .Include(ub => ub.Badge)
            .Where(ub => ub.UserId == currentUserId)
            .OrderByDescending(ub => ub.EarnedAt)
            .ToListAsync(cancellationToken);

        // Map to DTO
        var dto = new UserReputationDto
        {
            TotalPoints = reputation.TotalPoints,
            Level = reputation.Level,
            Rank = reputation.Rank,
            QuestionsAsked = reputation.QuestionsAsked,
            AnswersProvided = reputation.AnswersProvided,
            AcceptedAnswers = reputation.AcceptedAnswers,
            Badges = userBadges.Select(ub => new BadgeDto
            {
                Id = ub.Badge.Id,
                Name = ub.Badge.Name,
                Description = ub.Badge.Description,
                BadgeType = ub.Badge.BadgeType.ToString(),
                IconUrl = ub.Badge.IconUrl,
                EarnedAt = ub.EarnedAt
            }).ToList()
        };

        return Result<UserReputationDto>.Success(dto);
    }
}
