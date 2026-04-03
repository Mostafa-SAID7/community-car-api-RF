using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs.Admin;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Admin.Dashboard.Queries;

public class GetBusinessMetricsQueryHandler : IRequestHandler<GetBusinessMetricsQuery, Result<BusinessMetricsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBusinessMetricsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<BusinessMetricsDto>> Handle(GetBusinessMetricsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var startDate = now.AddMonths(-request.MonthsBack);

        var bookings = await _context.Bookings
            .Where(b => b.CreatedAt >= startDate)
            .ToListAsync(cancellationToken);

        var completedBookings = bookings.Where(b => b.Status == BookingStatus.Completed).ToList();
        var totalRevenue = completedBookings.Sum(b => b.TotalAmount);
        var averageBookingValue = completedBookings.Any() ? completedBookings.Average(b => b.TotalAmount) : 0;

        var cancelledBookings = bookings.Count(b => b.Status == BookingStatus.Cancelled);
        var completionRate = bookings.Any() 
            ? (double)completedBookings.Count / bookings.Count * 100 
            : 0;

        // Monthly revenues
        var monthlyRevenues = completedBookings
            .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
            .Select(g => new MonthlyRevenueDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Revenue = g.Sum(b => b.TotalAmount),
                BookingCount = g.Count()
            })
            .OrderBy(m => m.Month)
            .ToList();

        // Top cars by revenue
        var topCars = await _context.Bookings
            .Where(b => b.Status == BookingStatus.Completed && b.CreatedAt >= startDate)
            .GroupBy(b => b.CarId)
            .Select(g => new
            {
                CarId = g.Key,
                BookingCount = g.Count(),
                TotalRevenue = g.Sum(b => b.TotalAmount)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .Take(10)
            .ToListAsync(cancellationToken);

        var carIds = topCars.Select(t => t.CarId).ToList();
        var cars = await _context.Cars
            .Where(c => carIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var topCarDtos = topCars.Select(t =>
        {
            var car = cars.FirstOrDefault(c => c.Id == t.CarId);
            return new TopCarDto
            {
                CarId = t.CarId,
                Make = car?.Make ?? "Unknown",
                Model = car?.Model ?? "Unknown",
                BookingCount = t.BookingCount,
                TotalRevenue = t.TotalRevenue
            };
        }).ToList();

        var metrics = new BusinessMetricsDto
        {
            TotalRevenue = totalRevenue,
            AverageBookingValue = Math.Round(averageBookingValue, 2),
            TotalBookings = bookings.Count,
            CompletedBookings = completedBookings.Count,
            CancelledBookings = cancelledBookings,
            BookingCompletionRate = Math.Round(completionRate, 2),
            MonthlyRevenues = monthlyRevenues,
            TopCars = topCarDtos
        };

        return Result<BusinessMetricsDto>.Success(metrics);
    }
}
