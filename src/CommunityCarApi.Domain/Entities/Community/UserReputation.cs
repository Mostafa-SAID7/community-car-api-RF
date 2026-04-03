using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Entities.Identity;

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
