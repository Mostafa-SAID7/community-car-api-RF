using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Entities.Identity;

namespace CommunityCarApi.Domain.Entities.Community;

public class UserBadge : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid BadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}
