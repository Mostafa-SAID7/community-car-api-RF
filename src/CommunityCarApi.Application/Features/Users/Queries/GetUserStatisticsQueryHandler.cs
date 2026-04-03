using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Users.Queries;

public class GetUserStatisticsQueryHandler : IRequestHandler<GetUserStatisticsQuery, Result<UserStatisticsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserStatisticsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UserStatisticsDto>> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<UserStatisticsDto>.Failure("User not authenticated");

        var bookings = await _context.Bookings
            .Where(b => b.UserId == userId && !b.IsDeleted)
            .ToListAsync(cancellationToken);

        var cars = await _context.Cars
            .Where(c => c.OwnerId == userId && !c.IsDeleted)
            .CountAsync(cancellationToken);

        var statistics = new UserStatisticsDto
        {
            TotalBookings = bookings.Count,
            ActiveBookings = bookings.Count(b => b.Status == BookingStatus.Confirmed),
            CompletedBookings = bookings.Count(b => b.Status == BookingStatus.Completed),
            CancelledBookings = bookings.Count(b => b.Status == BookingStatus.Cancelled),
            TotalSpent = bookings.Where(b => b.Status == BookingStatus.Completed).Sum(b => b.TotalAmount),
            CarsOwned = cars
        };

        return Result<UserStatisticsDto>.Success(statistics);
    }
}
