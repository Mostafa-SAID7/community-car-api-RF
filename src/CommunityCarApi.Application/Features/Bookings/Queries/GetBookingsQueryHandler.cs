using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Bookings.Queries;

public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, Result<List<BookingDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetBookingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<BookingDto>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Bookings
            .Include(b => b.Car)
            .Where(b => !b.IsDeleted);

        if (request.CarId.HasValue)
            query = query.Where(b => b.CarId == request.CarId.Value);

        if (!string.IsNullOrEmpty(request.UserId))
            query = query.Where(b => b.UserId == request.UserId);

        if (request.Status.HasValue)
            query = query.Where(b => b.Status == (BookingStatus)request.Status.Value);

        if (request.StartDate.HasValue)
            query = query.Where(b => b.StartDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(b => b.EndDate <= request.EndDate.Value);

        var bookings = await query
            .Select(b => new BookingDto
            {
                Id = b.Id,
                BookingNumber = b.BookingNumber,
                CarId = b.CarId,
                UserId = b.UserId,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status.ToString(),
                PaymentStatus = b.PaymentStatus.ToString(),
                Notes = b.Notes,
                Car = new CarDto
                {
                    Id = b.Car.Id,
                    Make = b.Car.Make,
                    Model = b.Car.Model,
                    Year = b.Car.Year,
                    ImageUrl = b.Car.ImageUrl
                }
            })
            .OrderByDescending(b => b.StartDate)
            .ToListAsync(cancellationToken);

        return Result<List<BookingDto>>.Success(bookings);
    }
}
