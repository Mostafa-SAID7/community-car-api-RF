namespace CommunityCarApi.Application.DTOs;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string ReviewerId { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public Guid? CarId { get; set; }
    public string? ReviewedUserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateReviewDto
{
    public Guid? CarId { get; set; }
    public string? ReviewedUserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class ReviewStatisticsDto
{
    public int TotalReviews { get; set; }
    public double AverageRating { get; set; }
    public int FiveStars { get; set; }
    public int FourStars { get; set; }
    public int ThreeStars { get; set; }
    public int TwoStars { get; set; }
    public int OneStar { get; set; }
}
