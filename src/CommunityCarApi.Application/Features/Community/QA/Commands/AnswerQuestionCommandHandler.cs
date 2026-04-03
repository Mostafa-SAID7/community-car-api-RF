using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Domain.Entities.Community;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AnswerQuestionCommandHandler : IRequestHandler<AnswerQuestionCommand, Result<AnswerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGamificationService _gamificationService;

    public AnswerQuestionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IGamificationService gamificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _gamificationService = gamificationService;
    }

    public async Task<Result<AnswerDto>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<AnswerDto>.Failure("User is not authenticated");
        }

        // Check if question exists
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId && !q.IsDeleted, cancellationToken);

        if (question == null)
        {
            return Result<AnswerDto>.Failure("Question not found", ErrorCodes.NotFound);
        }

        // Check if user already answered this question
        var existingAnswer = await _context.Answers
            .AnyAsync(a => a.QuestionId == request.QuestionId 
                && a.UserId == userId 
                && !a.IsDeleted, cancellationToken);

        if (existingAnswer)
        {
            return Result<AnswerDto>.Failure("You have already answered this question", ErrorCodes.Conflict);
        }

        // Create the answer
        var answer = new Answer
        {
            QuestionId = request.QuestionId,
            UserId = userId,
            Content = request.Content,
            VoteCount = 0,
            IsAccepted = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Answers.Add(answer);

        // Increment question's answer count
        question.AnswerCount++;

        await _context.SaveChangesAsync(cancellationToken);

        // Award points for creating an answer
        await _gamificationService.AwardPointsForAnswerAsync(userId, cancellationToken);
        await _gamificationService.UpdateLevelAndRankAsync(userId, cancellationToken);
        await _gamificationService.CheckAndAwardBadgesAsync(userId, cancellationToken);

        // Get user information for the response
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        // Map to DTO
        var answerDto = new AnswerDto
        {
            Id = answer.Id,
            QuestionId = answer.QuestionId,
            UserId = answer.UserId,
            UserName = user?.UserName ?? string.Empty,
            Content = answer.Content,
            VoteCount = answer.VoteCount,
            IsAccepted = answer.IsAccepted,
            CreatedAt = answer.CreatedAt,
            HasUserVoted = false,
            UserVoteType = null
        };

        return Result<AnswerDto>.Success(answerDto);
    }
}
