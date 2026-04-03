using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Entities.Identity;

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
