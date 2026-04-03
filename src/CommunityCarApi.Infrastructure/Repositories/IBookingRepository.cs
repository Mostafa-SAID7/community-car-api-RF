using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Domain.Enums;

namespace CommunityCarApi.Infrastructure.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default);
    Task<bool> HasConflictingBookingAsync(Guid carId, DateTime startDate, DateTime endDate, Guid? excludeBookingId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default);
    Task<string> GenerateBookingNumberAsync(CancellationToken cancellationToken = default);
}
