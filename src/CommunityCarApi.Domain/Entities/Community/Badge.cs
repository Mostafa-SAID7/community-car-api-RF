using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Enums;

namespace CommunityCarApi.Domain.Entities.Community;

public class Badge : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BadgeType BadgeType { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}
