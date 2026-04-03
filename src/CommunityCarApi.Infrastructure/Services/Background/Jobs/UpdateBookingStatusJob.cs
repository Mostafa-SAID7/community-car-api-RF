using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunityCarApi.Infrastructure.Services.Background.Jobs;

public class UpdateBookingStatusJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateBookingStatusJob> _logger;
    private readonly IDateTime _dateTime;

    public UpdateBookingStatusJob(
        IApplicationDbContext context, 
        ILogger<UpdateBookingStatusJob> logger,
        IDateTime dateTime)
    {
        _context = context;
        _logger = logger;
        _dateTime = dateTime;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Starting booking status update job");

            var now = _dateTime.UtcNow;

            // Update bookings that should be completed
            var completedBookings = await _context.Bookings
                .Where(b => b.EndDate < now && 
                           b.Status == BookingStatus.Confirmed)
                .ToListAsync();

            foreach (var booking in completedBookings)
            {
                booking.Status = BookingStatus.Completed;
                booking.UpdatedAt = now;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated {Count} bookings to completed status", completedBookings.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking statuses");
        }
    }
}
