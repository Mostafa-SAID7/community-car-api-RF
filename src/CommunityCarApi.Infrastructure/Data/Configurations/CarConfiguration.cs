using CommunityCarApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Make)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Year)
            .IsRequired();

        builder.Property(e => e.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Color)
            .HasMaxLength(50);

        builder.Property(e => e.HourlyRate)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.DailyRate)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(e => e.LicensePlate)
            .IsUnique();

        builder.HasIndex(e => e.OwnerId);

        builder.HasIndex(e => new { e.City, e.State });

        builder.HasIndex(e => e.IsAvailable);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasMany(e => e.Bookings)
            .WithOne(b => b.Car)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
