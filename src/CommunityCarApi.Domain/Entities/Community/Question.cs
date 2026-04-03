using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Entities.Identity;
using CommunityCarApi.Domain.Enums;

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
