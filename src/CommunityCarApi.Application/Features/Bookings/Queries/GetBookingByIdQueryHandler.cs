using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Bookings.Queries;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBookingByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .Include(b => b.Car)
            .Where(b => b.Id == request.Id && !b.IsDeleted)
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
                    Color = b.Car.Color,
                    CarType = b.Car.CarType.ToString(),
                    FuelType = b.Car.FuelType.ToString(),
                    Transmission = b.Car.Transmission.ToString(),
                    Seats = b.Car.Seats,
                    HourlyRate = b.Car.HourlyRate,
                    DailyRate = b.Car.DailyRate,
                    City = b.Car.City,
                    State = b.Car.State,
                    ImageUrl = b.Car.ImageUrl
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        return Result<BookingDto>.Success(booking);
    }
}
