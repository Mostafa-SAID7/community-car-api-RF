using CommunityCarApi.Domain.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(e => e.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.Tags)
            .HasMaxLength(500);

        builder.Property(e => e.VoteCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.AnswerCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.ViewCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.IsSolved)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.Category);

        builder.HasIndex(e => e.CreatedAt);

        builder.HasIndex(e => e.VoteCount);

        builder.HasIndex(e => e.IsSolved);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Votes)
            .WithOne(v => v.Question)
            .HasForeignKey(v => v.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
