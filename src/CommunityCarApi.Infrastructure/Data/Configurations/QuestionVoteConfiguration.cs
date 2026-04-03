using CommunityCarApi.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class QuestionVoteConfiguration : IEntityTypeConfiguration<QuestionVote>
{
    public void Configure(EntityTypeBuilder<QuestionVote> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.QuestionId)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.VoteType)
            .IsRequired()
            .HasConversion<int>();

        // Unique constraint: one vote per user per question
        builder.HasIndex(e => new { e.QuestionId, e.UserId })
            .IsUnique();

        // Indexes
        builder.HasIndex(e => e.QuestionId);

        builder.HasIndex(e => e.UserId);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.Question)
            .WithMany(q => q.Votes)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
