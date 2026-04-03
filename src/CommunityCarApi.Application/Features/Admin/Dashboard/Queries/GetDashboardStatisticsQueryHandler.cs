using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Admin;
using CommunityCarApi.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Admin.Dashboard.Queries;

public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, Result<DashboardStatisticsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DashboardStatisticsDto>> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

        // Get total counts
        var totalUsers = await _context.Users.CountAsync(cancellationToken);
        var totalCars = await _context.Cars.CountAsync(cancellationToken);
        var totalBookings = await _context.Bookings.CountAsync(cancellationToken);
        var totalReviews = await _context.Reviews.CountAsync(cancellationToken);

        // Get active and pending bookings
        var activeBookings = await _context.Bookings
            .CountAsync(b => b.Status == Domain.Enums.BookingStatus.Confirmed, cancellationToken);

        var pendingBookings = await _context.Bookings
            .CountAsync(b => b.Status == Domain.Enums.BookingStatus.Pending, cancellationToken);

        // Calculate revenue
        var totalRevenue = await _context.Bookings
            .Where(b => b.Status == Domain.Enums.BookingStatus.Completed)
            .SumAsync(b => b.TotalAmount, cancellationToken);

        var monthlyRevenue = await _context.Bookings
            .Where(b => b.Status == Domain.Enums.BookingStatus.Completed && 
                       b.CreatedAt >= firstDayOfMonth)
            .SumAsync(b => b.TotalAmount, cancellationToken);

        // Get new users and cars this month
        var newUsersThisMonth = await _context.Users
            .CountAsync(u => u.CreatedAt >= firstDayOfMonth, cancellationToken);

        var newCarsThisMonth = await _context.Cars
            .CountAsync(c => c.CreatedAt >= firstDayOfMonth, cancellationToken);

        // Calculate average rating
        var reviews = await _context.Reviews.ToListAsync(cancellationToken);
        var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

        var statistics = new DashboardStatisticsDto
        {
            TotalUsers = totalUsers,
            TotalCars = totalCars,
            TotalBookings = totalBookings,
            TotalReviews = totalReviews,
            ActiveBookings = activeBookings,
            PendingBookings = pendingBookings,
            TotalRevenue = totalRevenue,
            MonthlyRevenue = monthlyRevenue,
            NewUsersThisMonth = newUsersThisMonth,
            NewCarsThisMonth = newCarsThisMonth,
            AverageRating = Math.Round(averageRating, 2)
        };

        return Result<DashboardStatisticsDto>.Success(statistics);
    }
}
