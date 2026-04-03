using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Entities.Identity;
using CommunityCarApi.Domain.Enums;

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
