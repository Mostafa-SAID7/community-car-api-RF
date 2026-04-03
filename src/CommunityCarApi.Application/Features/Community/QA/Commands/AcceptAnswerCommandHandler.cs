using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AcceptAnswerCommandHandler : IRequestHandler<AcceptAnswerCommand, Result<AnswerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGamificationService _gamificationService;

    public AcceptAnswerCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IGamificationService gamificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _gamificationService = gamificationService;
    }

    public async Task<Result<AnswerDto>> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<AnswerDto>.Failure("User is not authenticated");
        }

        // Get the answer with its question
        var answer = await _context.Answers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(a => a.Id == request.AnswerId && !a.IsDeleted, cancellationToken);

        if (answer == null)
        {
            return Result<AnswerDto>.Failure("Answer not found", ErrorCodes.NotFound);
        }

        // Check if the current user is the question author
        if (answer.Question.UserId != userId)
        {
            return Result<AnswerDto>.Failure("Only the question author can accept answers", ErrorCodes.Forbidden);
        }

        // If there's a previously accepted answer, unaccept it
        var previouslyAcceptedAnswer = await _context.Answers
            .FirstOrDefaultAsync(a => a.QuestionId == answer.QuestionId 
                && a.IsAccepted 
                && !a.IsDeleted, cancellationToken);

        if (previouslyAcceptedAnswer != null)
        {
            previouslyAcceptedAnswer.IsAccepted = false;
        }

        // Accept the new answer
        answer.IsAccepted = true;

        // Mark the question as solved
        answer.Question.IsSolved = true;

        await _context.SaveChangesAsync(cancellationToken);

        // Award points to the answer author
        await _gamificationService.AwardPointsForAcceptedAnswerAsync(answer.UserId, cancellationToken);
        await _gamificationService.UpdateLevelAndRankAsync(answer.UserId, cancellationToken);
        await _gamificationService.CheckAndAwardBadgesAsync(answer.UserId, cancellationToken);

        // Get user information for the response
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == answer.UserId, cancellationToken);

        // Check if current user has voted on this answer
        var userVote = await _context.AnswerVotes
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.AnswerId == answer.Id && v.UserId == userId, cancellationToken);

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
            HasUserVoted = userVote != null,
            UserVoteType = userVote != null ? (int)userVote.VoteType : null
        };

        return Result<AnswerDto>.Success(answerDto);
    }
}
