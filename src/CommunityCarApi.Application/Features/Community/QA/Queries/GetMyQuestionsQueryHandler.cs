using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.Common.Models;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetMyQuestionsQueryHandler : IRequestHandler<GetMyQuestionsQuery, Result<PaginatedList<QuestionDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyQuestionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedList<QuestionDto>>> Handle(GetMyQuestionsQuery request, CancellationToken cancellationToken)
    {
        // Get current user ID
        var currentUserId = _currentUserService.UserId;
        
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Result<PaginatedList<QuestionDto>>.Failure("User is not authenticated");
        }

        // Query questions created by the current user
        var query = _context.Questions
            .Where(q => !q.IsDeleted && q.UserId == currentUserId)
            .OrderByDescending(q => q.CreatedAt);

        // Project to DTO
        var dtoQuery = query.Select(q => new QuestionDto
        {
            Id = q.Id,
            UserId = q.UserId,
            UserName = q.User.UserName ?? string.Empty,
            Title = q.Title,
            Content = q.Content,
            Category = q.Category.ToString(),
            Tags = q.Tags != null 
                ? q.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>(),
            VoteCount = q.VoteCount,
            AnswerCount = q.AnswerCount,
            ViewCount = q.ViewCount,
            IsSolved = q.IsSolved,
            CreatedAt = q.CreatedAt
        });

        // Apply pagination
        var paginatedList = await PaginatedList<QuestionDto>.CreateAsync(
            dtoQuery, 
            request.PageNumber, 
            request.PageSize);

        return Result<PaginatedList<QuestionDto>>.Success(paginatedList);
    }
}
