using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.Common.Models;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, Result<PaginatedList<LeaderboardEntryDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetLeaderboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<LeaderboardEntryDto>>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        // Start with users who have reputation records and more than 0 points
        var query = _context.UserReputations
            .Where(ur => ur.TotalPoints > 0)
            .OrderByDescending(ur => ur.TotalPoints)
            .AsQueryable();

        // Project to DTO with badge count
        var dtoQuery = query.Select(ur => new LeaderboardEntryDto
        {
            UserId = ur.UserId,
            UserName = ur.User.UserName ?? string.Empty,
            TotalPoints = ur.TotalPoints,
            Level = ur.Level,
            Rank = ur.Rank,
            BadgeCount = ur.Badges.Count,
            QuestionsAsked = ur.QuestionsAsked,
            AnswersProvided = ur.AnswersProvided,
            AcceptedAnswers = ur.AcceptedAnswers
        });

        // Apply pagination
        var paginatedList = await PaginatedList<LeaderboardEntryDto>.CreateAsync(
            dtoQuery,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedList<LeaderboardEntryDto>>.Success(paginatedList);
    }
}
