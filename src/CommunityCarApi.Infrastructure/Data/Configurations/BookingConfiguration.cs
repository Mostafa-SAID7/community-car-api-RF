using CommunityCarApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityCarApi.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.BookingNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.PaymentStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(e => e.BookingNumber)
            .IsUnique();

        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => e.CarId);

        builder.HasIndex(e => new { e.StartDate, e.EndDate });

        builder.HasIndex(e => e.Status);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.Car)
            .WithMany(c => c.Bookings)
            .HasForeignKey(e => e.CarId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
