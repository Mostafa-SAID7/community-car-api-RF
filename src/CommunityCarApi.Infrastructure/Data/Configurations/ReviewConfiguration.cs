using CommunityCarApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ReviewerId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(e => e.ReviewedUserId)
            .HasMaxLength(450);

        builder.Property(e => e.Rating)
            .IsRequired();

        builder.Property(e => e.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(e => e.ReviewerId);

        builder.HasIndex(e => e.CarId);

        builder.HasIndex(e => e.ReviewedUserId);

        builder.HasIndex(e => e.Rating);

        builder.HasIndex(e => new { e.CarId, e.ReviewerId });

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.Car)
            .WithMany()
            .HasForeignKey(e => e.CarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
