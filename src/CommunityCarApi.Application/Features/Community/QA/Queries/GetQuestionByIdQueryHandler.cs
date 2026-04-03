using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Result<QuestionDetailDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetQuestionByIdQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<QuestionDetailDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(q => q.User)
            .Include(q => q.Answers.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.User)
            .Include(q => q.Answers.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.Votes)
            .Include(q => q.Votes)
            .Where(q => q.Id == request.QuestionId && !q.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (question == null)
        {
            return Result<QuestionDetailDto>.Failure("QA_QUESTION_NOT_FOUND", "Question not found.");
        }

        question.ViewCount++;
        await _context.SaveChangesAsync(cancellationToken);

        var currentUserId = _currentUserService.UserId;
        var userQuestionVote = question.Votes.FirstOrDefault(v => v.UserId == currentUserId);

        var questionDetailDto = new QuestionDetailDto
        {
            Id = question.Id,
            UserId = question.UserId,
            UserName = question.User.UserName ?? string.Empty,
            Title = question.Title,
            Content = question.Content,
            Category = question.Category.ToString(),
            Tags = question.Tags != null
                ? question.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>(),
            VoteCount = question.VoteCount,
            AnswerCount = question.AnswerCount,
            ViewCount = question.ViewCount,
            IsSolved = question.IsSolved,
            CreatedAt = question.CreatedAt,
            HasUserVoted = userQuestionVote != null,
            UserVoteType = userQuestionVote != null ? (int)userQuestionVote.VoteType : null,
            Answers = question.Answers
                .OrderByDescending(a => a.IsAccepted)
                .ThenByDescending(a => a.VoteCount)
                .ThenBy(a => a.CreatedAt)
                .Select(a =>
                {
                    var userAnswerVote = a.Votes.FirstOrDefault(v => v.UserId == currentUserId);
                    return new AnswerDto
                    {
                        Id = a.Id,
                        QuestionId = a.QuestionId,
                        UserId = a.UserId,
                        UserName = a.User.UserName ?? string.Empty,
                        Content = a.Content,
                        VoteCount = a.VoteCount,
                        IsAccepted = a.IsAccepted,
                        CreatedAt = a.CreatedAt,
                        HasUserVoted = userAnswerVote != null,
                        UserVoteType = userAnswerVote != null ? (int)userAnswerVote.VoteType : null
                    };
                })
                .ToList()
        };

        return Result<QuestionDetailDto>.Success(questionDetailDto);
    }
}
