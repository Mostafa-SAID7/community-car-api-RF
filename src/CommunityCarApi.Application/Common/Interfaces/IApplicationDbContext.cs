using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Domain.Entities.Community;
using CommunityCarApi.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Car> Cars { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<Review> Reviews { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<ApplicationUser> Users { get; }
    
    // Community Q&A System
    DbSet<Question> Questions { get; }
    DbSet<Answer> Answers { get; }
    DbSet<QuestionVote> QuestionVotes { get; }
    DbSet<AnswerVote> AnswerVotes { get; }
    DbSet<UserReputation> UserReputations { get; }
    DbSet<Badge> Badges { get; }
    DbSet<UserBadge> UserBadges { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
