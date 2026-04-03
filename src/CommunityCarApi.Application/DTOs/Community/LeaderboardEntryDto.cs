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
