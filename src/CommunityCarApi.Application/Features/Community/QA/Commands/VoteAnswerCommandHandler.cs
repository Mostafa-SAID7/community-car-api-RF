using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Domain.Entities.Community;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class VoteAnswerCommandHandler : IRequestHandler<VoteAnswerCommand, Result<VoteResultDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGamificationService _gamificationService;

    public VoteAnswerCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IGamificationService gamificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _gamificationService = gamificationService;
    }

    public async Task<Result<VoteResultDto>> Handle(VoteAnswerCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<VoteResultDto>.Failure("User is not authenticated");
        }

        // Get the answer
        var answer = await _context.Answers
            .FirstOrDefaultAsync(a => a.Id == request.AnswerId && !a.IsDeleted, cancellationToken);

        if (answer == null)
        {
            return Result<VoteResultDto>.Failure("QA_ANSWER_NOT_FOUND", "Answer not found");
        }

        // Prevent users from voting on their own answers
        if (answer.UserId == userId)
        {
            return Result<VoteResultDto>.Failure("QA_CANNOT_VOTE_OWN", "Cannot vote on your own answer");
        }

        // Check if user has already voted
        var existingVote = await _context.AnswerVotes
            .FirstOrDefaultAsync(v => v.AnswerId == request.AnswerId && v.UserId == userId, cancellationToken);

        var newVoteType = (VoteType)request.VoteType;
        int pointsAwarded = 0;

        if (existingVote == null)
        {
            // New vote
            var vote = new AnswerVote
            {
                AnswerId = request.AnswerId,
                UserId = userId,
                VoteType = newVoteType,
                CreatedAt = DateTime.UtcNow
            };

            _context.AnswerVotes.Add(vote);

            // Update answer vote count
            if (newVoteType == VoteType.Upvote)
            {
                answer.VoteCount += 1;
                pointsAwarded = await _gamificationService.AwardPointsForAnswerUpvoteAsync(answer.UserId, cancellationToken);
            }
            else
            {
                answer.VoteCount -= 1;
                pointsAwarded = await _gamificationService.DeductPointsForAnswerDownvoteAsync(answer.UserId, cancellationToken);
            }
        }
        else if (existingVote.VoteType == newVoteType)
        {
            // Remove vote (user clicked same vote type again)
            _context.AnswerVotes.Remove(existingVote);

            // Revert vote count
            if (newVoteType == VoteType.Upvote)
            {
                answer.VoteCount -= 1;
                pointsAwarded = await _gamificationService.DeductPointsForAnswerDownvoteAsync(answer.UserId, cancellationToken);
            }
            else
            {
                answer.VoteCount += 1;
                pointsAwarded = await _gamificationService.AwardPointsForAnswerUpvoteAsync(answer.UserId, cancellationToken);
            }
        }
        else
        {
            // Change vote (upvote to downvote or vice versa)
            existingVote.VoteType = newVoteType;

            // Update vote count (changes by 2)
            if (newVoteType == VoteType.Upvote)
            {
                answer.VoteCount += 2;
                // Changing from downvote to upvote: revert downvote penalty, then award upvote
                var revertPoints = await _gamificationService.AwardPointsForAnswerUpvoteAsync(answer.UserId, cancellationToken);
                var awardPoints = await _gamificationService.AwardPointsForAnswerUpvoteAsync(answer.UserId, cancellationToken);
                pointsAwarded = revertPoints + awardPoints;
            }
            else
            {
                answer.VoteCount -= 2;
                // Changing from upvote to downvote: revert upvote reward, then apply downvote penalty
                var revertPoints = await _gamificationService.DeductPointsForAnswerDownvoteAsync(answer.UserId, cancellationToken);
                var deductPoints = await _gamificationService.DeductPointsForAnswerDownvoteAsync(answer.UserId, cancellationToken);
                pointsAwarded = revertPoints + deductPoints;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Update level, rank, and check for badges
        await _gamificationService.UpdateLevelAndRankAsync(answer.UserId, cancellationToken);
        await _gamificationService.CheckAndAwardBadgesAsync(answer.UserId, cancellationToken);

        var result = new VoteResultDto
        {
            NewVoteCount = answer.VoteCount,
            PointsAwarded = pointsAwarded
        };

        return Result<VoteResultDto>.Success(result);
    }
}
