namespace CommunityCarApi.Application.DTOs.Community;

public class QuestionDetailDto : QuestionDto
{
    public List<AnswerDto> Answers { get; set; } = new();
    public bool HasUserVoted { get; set; }
    public int? UserVoteType { get; set; }
}
