using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Domain.Enums;
using CommunityCarApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Infrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => !b.IsDeleted && b.UserId == userId)
            .Include(b => b.Car)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => !b.IsDeleted && b.CarId == carId)
            .OrderByDescending(b => b.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasConflictingBookingAsync(
        Guid carId, 
        DateTime startDate, 
        DateTime endDate, 
        Guid? excludeBookingId = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(b => 
            !b.IsDeleted && 
            b.CarId == carId &&
            b.Status != BookingStatus.Cancelled &&
            (
                (b.StartDate <= startDate && b.EndDate >= startDate) ||
                (b.StartDate <= endDate && b.EndDate >= endDate) ||
                (b.StartDate >= startDate && b.EndDate <= endDate)
            )
        );

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => !b.IsDeleted && b.Status == status)
            .Include(b => b.Car)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateBookingNumberAsync(CancellationToken cancellationToken = default)
    {
        var date = DateTime.UtcNow;
        var prefix = $"BK{date:yyyyMMdd}";
        
        var lastBooking = await _dbSet
            .Where(b => b.BookingNumber.StartsWith(prefix))
            .OrderByDescending(b => b.BookingNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastBooking == null)
        {
            return $"{prefix}0001";
        }

        var lastNumber = int.Parse(lastBooking.BookingNumber.Substring(prefix.Length));
        var newNumber = lastNumber + 1;

        return $"{prefix}{newNumber:D4}";
    }
}
