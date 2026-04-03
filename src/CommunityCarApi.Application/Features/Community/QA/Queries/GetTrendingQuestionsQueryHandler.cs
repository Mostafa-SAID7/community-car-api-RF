using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetTrendingQuestionsQueryHandler : IRequestHandler<GetTrendingQuestionsQuery, Result<List<QuestionDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public GetTrendingQuestionsQueryHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Result<List<QuestionDto>>> Handle(GetTrendingQuestionsQuery request, CancellationToken cancellationToken)
    {
        // Calculate the date 7 days ago
        var sevenDaysAgo = _dateTime.UtcNow.AddDays(-7);

        // Get questions from the last 7 days, excluding soft-deleted ones
        var trendingQuestions = await _context.Questions
            .Where(q => !q.IsDeleted && q.CreatedAt >= sevenDaysAgo)
            .Select(q => new
            {
                Question = q,
                // Calculate trending score: ViewCount + VoteCount * 2 + AnswerCount * 3
                TrendingScore = q.ViewCount + (q.VoteCount * 2) + (q.AnswerCount * 3)
            })
            .OrderByDescending(x => x.TrendingScore)
            .Take(request.Limit)
            .Select(x => new QuestionDto
            {
                Id = x.Question.Id,
                UserId = x.Question.UserId,
                UserName = x.Question.User.UserName ?? string.Empty,
                Title = x.Question.Title,
                Content = x.Question.Content,
                Category = x.Question.Category.ToString(),
                Tags = x.Question.Tags != null
                    ? x.Question.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>(),
                VoteCount = x.Question.VoteCount,
                AnswerCount = x.Question.AnswerCount,
                ViewCount = x.Question.ViewCount,
                IsSolved = x.Question.IsSolved,
                CreatedAt = x.Question.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<List<QuestionDto>>.Success(trendingQuestions);
    }
}
