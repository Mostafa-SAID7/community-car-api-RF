using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Bookings.Commands;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<BookingDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public CreateBookingCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<Result<BookingDto>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<BookingDto>.Failure("User not authenticated");

        // Validate dates
        if (request.StartDate < _dateTime.UtcNow)
            return Result<BookingDto>.Failure("Start date cannot be in the past");

        if (request.EndDate <= request.StartDate)
            return Result<BookingDto>.Failure("End date must be after start date");

        // Check if car exists and is available
        var car = await _context.Cars
            .FirstOrDefaultAsync(c => c.Id == request.CarId && !c.IsDeleted, cancellationToken);

        if (car == null)
            return Result<BookingDto>.Failure("Car not found");

        if (!car.IsAvailable)
            return Result<BookingDto>.Failure("Car is not available");

        // Check for conflicting bookings
        var hasConflict = await _context.Bookings
            .AnyAsync(b => b.CarId == request.CarId &&
                          b.Status != BookingStatus.Cancelled &&
                          ((request.StartDate >= b.StartDate && request.StartDate < b.EndDate) ||
                           (request.EndDate > b.StartDate && request.EndDate <= b.EndDate) ||
                           (request.StartDate <= b.StartDate && request.EndDate >= b.EndDate)),
                      cancellationToken);

        if (hasConflict)
            return Result<BookingDto>.Failure("Car is already booked for the selected dates");

        // Calculate total amount
        var days = (request.EndDate - request.StartDate).Days;
        var totalAmount = days * car.DailyRate;

        // Generate booking number
        var bookingNumber = $"BK{_dateTime.UtcNow:yyyyMMddHHmmss}";

        var booking = new Booking
        {
            BookingNumber = bookingNumber,
            CarId = request.CarId,
            UserId = userId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalAmount = totalAmount,
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            Notes = request.Notes
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);

        var bookingDto = new BookingDto
        {
            Id = booking.Id,
            BookingNumber = booking.BookingNumber,
            CarId = booking.CarId,
            UserId = booking.UserId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status.ToString(),
            PaymentStatus = booking.PaymentStatus.ToString(),
            Notes = booking.Notes
        };

        return Result<BookingDto>.Success(bookingDto);
    }
}
