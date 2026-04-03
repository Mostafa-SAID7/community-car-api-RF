using CommunityCarApi.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class UserBadgeConfiguration : IEntityTypeConfiguration<UserBadge>
{
    public void Configure(EntityTypeBuilder<UserBadge> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.BadgeId)
            .IsRequired();

        builder.Property(e => e.EarnedAt)
            .IsRequired();

        // Unique constraint: one badge per user
        builder.HasIndex(e => new { e.UserId, e.BadgeId })
            .IsUnique();

        // Indexes
        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.BadgeId);

        builder.HasIndex(e => e.EarnedAt);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Badge)
            .WithMany(b => b.UserBadges)
            .HasForeignKey(e => e.BadgeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
