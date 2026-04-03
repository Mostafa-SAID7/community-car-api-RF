using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.Common.Models;
using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, Result<PaginatedList<QuestionDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetQuestionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<QuestionDto>>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        // Start with non-deleted questions
        var query = _context.Questions
            .Where(q => !q.IsDeleted)
            .AsQueryable();

        // Apply category filter
        if (request.Category.HasValue)
        {
            var category = (QuestionCategory)request.Category.Value;
            query = query.Where(q => q.Category == category);
        }

        // Apply search term filter (title or content)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(q => q.Title.ToLower().Contains(searchTerm) || 
                                    q.Content.ToLower().Contains(searchTerm));
        }

        // Apply tag filter
        if (!string.IsNullOrWhiteSpace(request.Tag))
        {
            var tag = request.Tag.ToLower();
            query = query.Where(q => q.Tags != null && q.Tags.ToLower().Contains(tag));
        }

        // Apply IsSolved filter
        if (request.IsSolved.HasValue)
        {
            query = query.Where(q => q.IsSolved == request.IsSolved.Value);
        }

        // Apply sorting
        query = request.SortBy.ToLower() switch
        {
            "votecount" => request.SortDescending 
                ? query.OrderByDescending(q => q.VoteCount)
                : query.OrderBy(q => q.VoteCount),
            "answercount" => request.SortDescending
                ? query.OrderByDescending(q => q.AnswerCount)
                : query.OrderBy(q => q.AnswerCount),
            "viewcount" => request.SortDescending
                ? query.OrderByDescending(q => q.ViewCount)
                : query.OrderBy(q => q.ViewCount),
            _ => request.SortDescending
                ? query.OrderByDescending(q => q.CreatedAt)
                : query.OrderBy(q => q.CreatedAt)
        };

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
