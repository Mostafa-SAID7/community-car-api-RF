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
