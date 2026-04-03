using CommunityCarApi.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class AnswerVoteConfiguration : IEntityTypeConfiguration<AnswerVote>
{
    public void Configure(EntityTypeBuilder<AnswerVote> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.AnswerId)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.VoteType)
            .IsRequired()
            .HasConversion<int>();

        // Unique constraint: one vote per user per answer
        builder.HasIndex(e => new { e.AnswerId, e.UserId })
            .IsUnique();

        // Indexes
        builder.HasIndex(e => e.AnswerId);

        builder.HasIndex(e => e.UserId);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.Answer)
            .WithMany(a => a.Votes)
            .HasForeignKey(e => e.AnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
