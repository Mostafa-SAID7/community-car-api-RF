using CommunityCarApi.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.CreatedByIp)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RevokedByIp)
            .HasMaxLength(50);

        builder.Property(e => e.ReplacedByToken)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(e => e.Token);

        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.ExpiresAt);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
