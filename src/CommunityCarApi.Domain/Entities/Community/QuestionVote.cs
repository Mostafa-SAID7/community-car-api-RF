using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Entities.Identity;
using CommunityCarApi.Domain.Enums;

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
