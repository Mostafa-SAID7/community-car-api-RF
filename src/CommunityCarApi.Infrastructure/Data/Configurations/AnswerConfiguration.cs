using CommunityCarApi.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.QuestionId)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(e => e.VoteCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.IsAccepted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(e => e.QuestionId);

        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.CreatedAt);

        builder.HasIndex(e => e.VoteCount);

        builder.HasIndex(e => e.IsAccepted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Votes)
            .WithOne(v => v.Answer)
            .HasForeignKey(v => v.AnswerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
