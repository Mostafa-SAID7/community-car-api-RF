using CommunityCarApi.Domain.Entities.Community;

namespace CommunityCarApi.Application.Common.Interfaces;

public interface IGamificationService
{
    Task<int> AwardPointsForQuestionAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> AwardPointsForAnswerAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> AwardPointsForQuestionUpvoteAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> DeductPointsForQuestionDownvoteAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> AwardPointsForAnswerUpvoteAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> DeductPointsForAnswerDownvoteAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> AwardPointsForAcceptedAnswerAsync(string userId, CancellationToken cancellationToken = default);
    Task UpdateLevelAndRankAsync(string userId, CancellationToken cancellationToken = default);
    Task CheckAndAwardBadgesAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserReputation> GetOrCreateReputationAsync(string userId, CancellationToken cancellationToken = default);
}
