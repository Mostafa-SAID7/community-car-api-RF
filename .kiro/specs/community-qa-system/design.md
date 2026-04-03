# Design Document

## Introduction

This document provides the technical design for the Community Q&A System feature. The design follows Clean Architecture principles and integrates seamlessly with the existing CommunityCarApi codebase, utilizing CQRS with MediatR, Entity Framework Core, and the established Result pattern for error handling.

## Architecture Overview

The Community Q&A System will be implemented across multiple layers:

- **Domain Layer**: Core entities (Question, Answer, QuestionVote, AnswerVote, UserReputation, Badge, UserBadge)
- **Application Layer**: CQRS commands/queries, DTOs, validators, and gamification service interface
- **Infrastructure Layer**: Gamification service implementation, EF Core configurations, and data seeders
- **WebApi Layer**: QA Controller with RESTful endpoints

## Domain Model

### Entities

#### Question Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class Question : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public QuestionCategory Category { get; set; }
    public string? Tags { get; set; } // Comma-separated
    public int VoteCount { get; set; } = 0;
    public int AnswerCount { get; set; } = 0;
    public int ViewCount { get; set; } = 0;
    public bool IsSolved { get; set; } = false;
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<QuestionVote> Votes { get; set; } = new List<QuestionVote>();
}
```

#### Answer Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class Answer : BaseEntity
{
    public Guid QuestionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int VoteCount { get; set; } = 0;
    public bool IsAccepted { get; set; } = false;
    
    // Navigation properties
    public Question Question { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<AnswerVote> Votes { get; set; } = new List<AnswerVote>();
}
```

#### QuestionVote Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class QuestionVote : BaseEntity
{
    public Guid QuestionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public VoteType VoteType { get; set; }
    
    // Navigation properties
    public Question Question { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
```

#### AnswerVote Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class AnswerVote : BaseEntity
{
    public Guid AnswerId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public VoteType VoteType { get; set; }
    
    // Navigation properties
    public Answer Answer { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
```

#### UserReputation Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class UserReputation : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int TotalPoints { get; set; } = 0;
    public int Level { get; set; } = 1;
    public string Rank { get; set; } = "Beginner";
    public int QuestionsAsked { get; set; } = 0;
    public int AnswersProvided { get; set; } = 0;
    public int AcceptedAnswers { get; set; } = 0;
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();
}
```

#### Badge Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class Badge : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BadgeType BadgeType { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}
```

#### UserBadge Entity
```csharp
namespace CommunityCarApi.Domain.Entities.Community;

public class UserBadge : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid BadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}
```

### Enumerations

#### QuestionCategory Enum
```csharp
namespace CommunityCarApi.Domain.Enums;

public enum QuestionCategory
{
    General = 0,
    Maintenance = 1,
    BuyingGuide = 2,
    Troubleshooting = 3,
    Insurance = 4,
    Legal = 5,
    Safety = 6,
    Technology = 7,
    CostSharing = 8,
    BestPractices = 9
}
```

#### VoteType Enum
```csharp
namespace CommunityCarApi.Domain.Enums;

public enum VoteType
{
    Upvote = 1,
    Downvote = -1
}
```

#### BadgeType Enum
```csharp
namespace CommunityCarApi.Domain.Enums;

public enum BadgeType
{
    Contributor = 0,
    Expert = 1,
    Master = 2,
    ProblemSolver = 3,
    GreatQuestion = 4,
    GreatAnswer = 5
}
```

## Application Layer Design

### Commands

#### AskQuestionCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AskQuestionCommand : IRequest<Result<QuestionDto>>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Category { get; set; }
    public string? Tags { get; set; }
}
```

#### AnswerQuestionCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AnswerQuestionCommand : IRequest<Result<AnswerDto>>
{
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = string.Empty;
}
```

#### VoteQuestionCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class VoteQuestionCommand : IRequest<Result<VoteResultDto>>
{
    public Guid QuestionId { get; set; }
    public int VoteType { get; set; } // 1 for upvote, -1 for downvote
}
```

#### VoteAnswerCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class VoteAnswerCommand : IRequest<Result<VoteResultDto>>
{
    public Guid AnswerId { get; set; }
    public int VoteType { get; set; } // 1 for upvote, -1 for downvote
}
```

#### AcceptAnswerCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AcceptAnswerCommand : IRequest<Result<AnswerDto>>
{
    public Guid AnswerId { get; set; }
}
```

#### DeleteQuestionCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class DeleteQuestionCommand : IRequest<Result<bool>>
{
    public Guid QuestionId { get; set; }
}
```

#### DeleteAnswerCommand
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class DeleteAnswerCommand : IRequest<Result<bool>>
{
    public Guid AnswerId { get; set; }
}
```

### Queries

#### GetQuestionsQuery
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetQuestionsQuery : IRequest<Result<PaginatedList<QuestionDto>>>
{
    public int? Category { get; set; }
    public string? SearchTerm { get; set; }
    public string? Tag { get; set; }
    public bool? IsSolved { get; set; }
    public string SortBy { get; set; } = "CreatedAt"; // CreatedAt, VoteCount, AnswerCount, ViewCount
    public bool SortDescending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

#### GetQuestionByIdQuery
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetQuestionByIdQuery : IRequest<Result<QuestionDetailDto>>
{
    public Guid QuestionId { get; set; }
}
```

#### GetTrendingQuestionsQuery
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetTrendingQuestionsQuery : IRequest<Result<List<QuestionDto>>>
{
    public int Limit { get; set; } = 20;
}
```

#### GetMyQuestionsQuery
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetMyQuestionsQuery : IRequest<Result<PaginatedList<QuestionDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

#### GetLeaderboardQuery
```csharp
namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetLeaderboardQuery : IRequest<Result<PaginatedList<LeaderboardEntryDto>>>
{
    public string TimePeriod { get; set; } = "AllTime"; // AllTime, Monthly, Weekly
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
```

### DTOs

#### QuestionDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int VoteCount { get; set; }
    public int AnswerCount { get; set; }
    public int ViewCount { get; set; }
    public bool IsSolved { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### QuestionDetailDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class QuestionDetailDto : QuestionDto
{
    public List<AnswerDto> Answers { get; set; } = new();
    public bool HasUserVoted { get; set; }
    public int? UserVoteType { get; set; }
}
```

#### AnswerDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class AnswerDto
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int VoteCount { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasUserVoted { get; set; }
    public int? UserVoteType { get; set; }
}
```

#### VoteResultDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class VoteResultDto
{
    public int NewVoteCount { get; set; }
    public int PointsAwarded { get; set; }
}
```

#### LeaderboardEntryDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class LeaderboardEntryDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
    public int Level { get; set; }
    public string Rank { get; set; } = string.Empty;
    public int BadgeCount { get; set; }
    public int QuestionsAsked { get; set; }
    public int AnswersProvided { get; set; }
    public int AcceptedAnswers { get; set; }
}
```

#### UserReputationDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class UserReputationDto
{
    public int TotalPoints { get; set; }
    public int Level { get; set; }
    public string Rank { get; set; } = string.Empty;
    public int QuestionsAsked { get; set; }
    public int AnswersProvided { get; set; }
    public int AcceptedAnswers { get; set; }
    public List<BadgeDto> Badges { get; set; } = new();
}
```

#### BadgeDto
```csharp
namespace CommunityCarApi.Application.DTOs.Community;

public class BadgeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string BadgeType { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public DateTime EarnedAt { get; set; }
}
```

### Validators

#### AskQuestionCommandValidator
```csharp
- Title: Required, 10-200 characters
- Content: Required, 20-5000 characters
- Category: Must be valid enum value
- Tags: Optional, each tag 2-30 characters when split by comma
```

#### AnswerQuestionCommandValidator
```csharp
- QuestionId: Required, must be valid GUID
- Content: Required, 20-5000 characters
```

#### VoteQuestionCommandValidator
```csharp
- QuestionId: Required, must be valid GUID
- VoteType: Must be 1 or -1
```

#### VoteAnswerCommandValidator
```csharp
- AnswerId: Required, must be valid GUID
- VoteType: Must be 1 or -1
```

### Gamification Service Interface

```csharp
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
```

## Infrastructure Layer Design

### Gamification Service Implementation

The service will implement point calculations, level/rank updates, and badge awarding logic:

**Point Awards:**
- Question created: +5 points
- Answer created: +10 points
- Question upvoted: +5 points to author
- Question downvoted: -2 points to author
- Answer upvoted: +10 points to author
- Answer downvoted: -5 points to author
- Answer accepted: +25 points to author

**Level Calculation:**
- Level 1 (Beginner): 0-99 points
- Level 2 (Contributor): 100-499 points
- Level 3 (Expert): 500-999 points
- Level 4 (Master): 1000-2499 points
- Level 5 (Legend): 2500+ points

**Badge Awards:**
- Contributor: 100 total points
- Expert: 500 total points
- Master: 1000 total points
- Problem Solver: 10 accepted answers
- Great Question: Question with 50 upvotes
- Great Answer: Answer with 50 upvotes

### EF Core Configurations

Each entity will have a configuration class defining:
- Primary keys
- Required fields and max lengths
- Decimal precision for numeric fields
- Indexes for performance (UserId, QuestionId, CreatedAt, VoteCount, etc.)
- Unique constraints (one vote per user per question/answer)
- Relationships with proper cascade behavior
- Soft delete query filters

### Data Seeders

**BadgeSeeder**: Seeds the 6 badge types with descriptions and icons

## WebApi Layer Design

### QA Controller

**Base Route**: `/api/qa`

**Endpoints:**

1. **POST /api/qa/questions** - Ask a question
   - Authorization: Required (Authenticated users)
   - Request: AskQuestionCommand
   - Response: QuestionDto

2. **GET /api/qa/questions** - Get questions with filters
   - Authorization: None (Public)
   - Query params: category, searchTerm, tag, isSolved, sortBy, sortDescending, pageNumber, pageSize
   - Response: PaginatedList<QuestionDto>

3. **GET /api/qa/questions/{id}** - Get question details
   - Authorization: None (Public)
   - Response: QuestionDetailDto

4. **DELETE /api/qa/questions/{id}** - Delete question
   - Authorization: Required (Author or Admin)
   - Response: bool

5. **GET /api/qa/questions/trending** - Get trending questions
   - Authorization: None (Public)
   - Query params: limit
   - Response: List<QuestionDto>

6. **GET /api/qa/questions/my** - Get user's questions
   - Authorization: Required (Authenticated users)
   - Query params: pageNumber, pageSize
   - Response: PaginatedList<QuestionDto>

7. **POST /api/qa/answers** - Answer a question
   - Authorization: Required (Authenticated users)
   - Request: AnswerQuestionCommand
   - Response: AnswerDto

8. **DELETE /api/qa/answers/{id}** - Delete answer
   - Authorization: Required (Author or Admin)
   - Response: bool

9. **POST /api/qa/questions/{id}/vote** - Vote on question
   - Authorization: Required (Authenticated users)
   - Request: VoteQuestionCommand
   - Response: VoteResultDto

10. **POST /api/qa/answers/{id}/vote** - Vote on answer
    - Authorization: Required (Authenticated users)
    - Request: VoteAnswerCommand
    - Response: VoteResultDto

11. **POST /api/qa/answers/{id}/accept** - Accept answer
    - Authorization: Required (Question author)
    - Response: AnswerDto

12. **GET /api/qa/leaderboard** - Get leaderboard
    - Authorization: None (Public)
    - Query params: timePeriod, pageNumber, pageSize
    - Response: PaginatedList<LeaderboardEntryDto>

13. **GET /api/qa/reputation** - Get current user's reputation
    - Authorization: Required (Authenticated users)
    - Response: UserReputationDto

## Database Schema

### Tables

1. **Questions**
   - Primary Key: Id (Guid)
   - Foreign Key: UserId → AspNetUsers
   - Indexes: UserId, Category, CreatedAt, VoteCount, IsSolved
   - Soft Delete: IsDeleted, DeletedAt

2. **Answers**
   - Primary Key: Id (Guid)
   - Foreign Keys: QuestionId → Questions, UserId → AspNetUsers
   - Indexes: QuestionId, UserId, CreatedAt, VoteCount, IsAccepted
   - Soft Delete: IsDeleted, DeletedAt

3. **QuestionVotes**
   - Primary Key: Id (Guid)
   - Foreign Keys: QuestionId → Questions, UserId → AspNetUsers
   - Unique Constraint: (QuestionId, UserId)
   - Indexes: QuestionId, UserId

4. **AnswerVotes**
   - Primary Key: Id (Guid)
   - Foreign Keys: AnswerId → Answers, UserId → AspNetUsers
   - Unique Constraint: (AnswerId, UserId)
   - Indexes: AnswerId, UserId

5. **UserReputations**
   - Primary Key: Id (Guid)
   - Foreign Key: UserId → AspNetUsers (Unique)
   - Indexes: UserId, TotalPoints, Level

6. **Badges**
   - Primary Key: Id (Guid)
   - Indexes: BadgeType

7. **UserBadges**
   - Primary Key: Id (Guid)
   - Foreign Keys: UserId → AspNetUsers, BadgeId → Badges
   - Unique Constraint: (UserId, BadgeId)
   - Indexes: UserId, BadgeId, EarnedAt

### Relationships

- Question → User (Many-to-One)
- Question → Answers (One-to-Many, Cascade delete)
- Question → QuestionVotes (One-to-Many, Cascade delete)
- Answer → User (Many-to-One)
- Answer → Question (Many-to-One)
- Answer → AnswerVotes (One-to-Many, Cascade delete)
- UserReputation → User (One-to-One)
- UserReputation → UserBadges (One-to-Many)
- Badge → UserBadges (One-to-Many)

## Integration Points

### Authentication & Authorization
- Uses existing JWT authentication
- Leverages ICurrentUserService for user context
- Role-based authorization for admin operations

### Database Context
- Extends IApplicationDbContext with new DbSets
- Uses existing ApplicationDbContext infrastructure

### Caching
- Leaderboard results cached for 5 minutes using ICacheService
- Trending questions cached for 10 minutes

### Background Jobs
- Optional: Daily job to recalculate trending scores
- Optional: Weekly job to award time-based badges

## Error Handling

All operations use the Result pattern with standardized error codes:

- **QA_QUESTION_NOT_FOUND**: Question does not exist
- **QA_ANSWER_NOT_FOUND**: Answer does not exist
- **QA_UNAUTHORIZED_DELETE**: User cannot delete this content
- **QA_UNAUTHORIZED_ACCEPT**: Only question author can accept answers
- **QA_CANNOT_VOTE_OWN**: Cannot vote on own content
- **QA_DUPLICATE_ANSWER**: User already answered this question
- **QA_ALREADY_SOLVED**: Question already has accepted answer
- **QA_VALIDATION_ERROR**: Input validation failed

## Performance Considerations

1. **Indexes**: Strategic indexes on frequently queried columns
2. **Pagination**: All list endpoints support pagination
3. **Caching**: Leaderboard and trending questions cached
4. **Eager Loading**: Include related entities to avoid N+1 queries
5. **AsNoTracking**: Use for read-only queries
6. **Projections**: Select only needed fields in DTOs

## Testing Strategy

1. **Unit Tests**: Command/Query handlers, validators, gamification service
2. **Integration Tests**: Database operations, API endpoints
3. **Test Data**: Seed test questions, answers, and votes
4. **Edge Cases**: Voting on deleted content, accepting multiple answers, point boundaries

## Migration Plan

1. Create domain entities and enums
2. Update IApplicationDbContext with new DbSets
3. Create EF Core configurations
4. Generate and apply migration
5. Create badge seeder
6. Implement gamification service
7. Implement commands and queries
8. Create validators
9. Implement QA controller
10. Test all endpoints
11. Update API documentation

## Future Enhancements

- Question editing capability
- Answer editing capability
- Comment threads on answers
- Question/answer reporting system
- Rich text editor support
- Image attachments
- Email notifications for answers
- Search with full-text indexing
- Question categories with icons
- User mention system (@username)
