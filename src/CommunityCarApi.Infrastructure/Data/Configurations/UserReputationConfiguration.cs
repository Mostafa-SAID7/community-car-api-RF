using CommunityCarApi.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class UserReputationConfiguration : IEntityTypeConfiguration<UserReputation>
{
    public void Configure(EntityTypeBuilder<UserReputation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.TotalPoints)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.Level)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(e => e.Rank)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Beginner");

        builder.Property(e => e.QuestionsAsked)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.AnswersProvided)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.AcceptedAnswers)
            .IsRequired()
            .HasDefaultValue(0);

        // Unique constraint: one reputation record per user
        builder.HasIndex(e => e.UserId)
            .IsUnique();

        // Indexes
        builder.HasIndex(e => e.TotalPoints);

        builder.HasIndex(e => e.Level);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserReputation can access UserBadges through the shared UserId
        // This provides convenient access to badges without going through User
        builder.HasMany(e => e.Badges)
            .WithOne()
            .HasForeignKey(ub => ub.UserId)
            .HasPrincipalKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
