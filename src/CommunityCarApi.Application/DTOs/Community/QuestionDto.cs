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
