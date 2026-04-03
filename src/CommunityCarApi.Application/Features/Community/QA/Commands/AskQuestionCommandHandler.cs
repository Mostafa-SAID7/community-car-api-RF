using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Domain.Entities.Community;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AskQuestionCommandHandler : IRequestHandler<AskQuestionCommand, Result<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGamificationService _gamificationService;

    public AskQuestionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IGamificationService gamificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _gamificationService = gamificationService;
    }

    public async Task<Result<QuestionDto>> Handle(AskQuestionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<QuestionDto>.Failure("User is not authenticated");
        }

        // Create the question
        var question = new Question
        {
            UserId = userId,
            Title = request.Title,
            Content = request.Content,
            Category = (QuestionCategory)request.Category,
            Tags = request.Tags,
            VoteCount = 0,
            AnswerCount = 0,
            ViewCount = 0,
            IsSolved = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);

        // Award points for creating a question
        await _gamificationService.AwardPointsForQuestionAsync(userId, cancellationToken);
        await _gamificationService.UpdateLevelAndRankAsync(userId, cancellationToken);
        await _gamificationService.CheckAndAwardBadgesAsync(userId, cancellationToken);

        // Get user information for the response
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        // Map to DTO
        var questionDto = new QuestionDto
        {
            Id = question.Id,
            UserId = question.UserId,
            UserName = user?.UserName ?? string.Empty,
            Title = question.Title,
            Content = question.Content,
            Category = question.Category.ToString(),
            Tags = string.IsNullOrEmpty(question.Tags) 
                ? new List<string>() 
                : question.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
            VoteCount = question.VoteCount,
            AnswerCount = question.AnswerCount,
            ViewCount = question.ViewCount,
            IsSolved = question.IsSolved,
            CreatedAt = question.CreatedAt
        };

        return Result<QuestionDto>.Success(questionDto);
    }
}
