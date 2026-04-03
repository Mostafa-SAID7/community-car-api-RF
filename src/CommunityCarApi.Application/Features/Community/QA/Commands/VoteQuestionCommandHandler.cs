using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Domain.Entities.Community;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class VoteQuestionCommandHandler : IRequestHandler<VoteQuestionCommand, Result<VoteResultDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGamificationService _gamificationService;

    public VoteQuestionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IGamificationService gamificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _gamificationService = gamificationService;
    }

    public async Task<Result<VoteResultDto>> Handle(VoteQuestionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<VoteResultDto>.Failure("User is not authenticated");
        }

        // Get the question
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId && !q.IsDeleted, cancellationToken);

        if (question == null)
        {
            return Result<VoteResultDto>.Failure("QA_QUESTION_NOT_FOUND", "Question not found");
        }

        // Prevent users from voting on their own questions
        if (question.UserId == userId)
        {
            return Result<VoteResultDto>.Failure("QA_CANNOT_VOTE_OWN", "Cannot vote on your own question");
        }

        // Check if user has already voted
        var existingVote = await _context.QuestionVotes
            .FirstOrDefaultAsync(v => v.QuestionId == request.QuestionId && v.UserId == userId, cancellationToken);

        var newVoteType = (VoteType)request.VoteType;
        int pointsAwarded = 0;

        if (existingVote == null)
        {
            // New vote
            var vote = new QuestionVote
            {
                QuestionId = request.QuestionId,
                UserId = userId,
                VoteType = newVoteType,
                CreatedAt = DateTime.UtcNow
            };

            _context.QuestionVotes.Add(vote);

            // Update question vote count
            if (newVoteType == VoteType.Upvote)
            {
                question.VoteCount += 1;
                pointsAwarded = await _gamificationService.AwardPointsForQuestionUpvoteAsync(question.UserId, cancellationToken);
            }
            else
            {
                question.VoteCount -= 1;
                pointsAwarded = await _gamificationService.DeductPointsForQuestionDownvoteAsync(question.UserId, cancellationToken);
            }
        }
        else if (existingVote.VoteType == newVoteType)
        {
            // Remove vote (user clicked same vote type again)
            _context.QuestionVotes.Remove(existingVote);

            // Revert vote count
            if (newVoteType == VoteType.Upvote)
            {
                question.VoteCount -= 1;
                pointsAwarded = await _gamificationService.DeductPointsForQuestionDownvoteAsync(question.UserId, cancellationToken);
            }
            else
            {
                question.VoteCount += 1;
                pointsAwarded = await _gamificationService.AwardPointsForQuestionUpvoteAsync(question.UserId, cancellationToken);
            }
        }
        else
        {
            // Change vote (upvote to downvote or vice versa)
            existingVote.VoteType = newVoteType;

            // Update vote count (changes by 2)
            if (newVoteType == VoteType.Upvote)
            {
                question.VoteCount += 2;
                // Changing from downvote to upvote: revert downvote penalty, then award upvote
                var revertPoints = await _gamificationService.AwardPointsForQuestionUpvoteAsync(question.UserId, cancellationToken);
                var awardPoints = await _gamificationService.AwardPointsForQuestionUpvoteAsync(question.UserId, cancellationToken);
                pointsAwarded = revertPoints + awardPoints;
            }
            else
            {
                question.VoteCount -= 2;
                // Changing from upvote to downvote: revert upvote reward, then apply downvote penalty
                var revertPoints = await _gamificationService.DeductPointsForQuestionDownvoteAsync(question.UserId, cancellationToken);
                var deductPoints = await _gamificationService.DeductPointsForQuestionDownvoteAsync(question.UserId, cancellationToken);
                pointsAwarded = revertPoints + deductPoints;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Update level, rank, and check for badges
        await _gamificationService.UpdateLevelAndRankAsync(question.UserId, cancellationToken);
        await _gamificationService.CheckAndAwardBadgesAsync(question.UserId, cancellationToken);

        var result = new VoteResultDto
        {
            NewVoteCount = question.VoteCount,
            PointsAwarded = pointsAwarded
        };

        return Result<VoteResultDto>.Success(result);
    }
}
