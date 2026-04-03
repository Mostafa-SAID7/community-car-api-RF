using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Bookings.Commands;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public CancelBookingCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<Result<bool>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, cancellationToken);

        if (booking == null)
            return Result<bool>.Failure("Booking not found");

        // Check ownership or admin
        if (booking.UserId != userId && !_currentUserService.IsInRole("Admin"))
            return Result<bool>.Failure("You don't have permission to cancel this booking");

        // Check if booking can be cancelled
        if (booking.Status == BookingStatus.Cancelled)
            return Result<bool>.Failure("Booking is already cancelled");

        if (booking.Status == BookingStatus.Completed)
            return Result<bool>.Failure("Cannot cancel a completed booking");

        if (booking.StartDate < _dateTime.UtcNow)
            return Result<bool>.Failure("Cannot cancel a booking that has already started");

        booking.Status = BookingStatus.Cancelled;
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
