using CommunityCarApi.Domain.Common;

namespace CommunityCarApi.Domain.Entities;

public class Review : BaseEntity
{
    public string ReviewerId { get; set; } = string.Empty;
    public Guid? CarId { get; set; }
    public string? ReviewedUserId { get; set; }
    public int Rating { get; set; } // 1-5
    public string Comment { get; set; } = string.Empty;
    public bool IsVerified { get; set; } = false;
    
    // Navigation properties
    public Car? Car { get; set; }
}
