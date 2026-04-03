using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Reviews.Commands;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateReviewCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<Guid>.Failure("User not authenticated");

        // Validate that either CarId or ReviewedUserId is provided
        if (!request.CarId.HasValue && string.IsNullOrEmpty(request.ReviewedUserId))
            return Result<Guid>.Failure("Either CarId or ReviewedUserId must be provided");

        // Validate that both are not provided
        if (request.CarId.HasValue && !string.IsNullOrEmpty(request.ReviewedUserId))
            return Result<Guid>.Failure("Cannot review both a car and a user in the same review");

        // If reviewing a car, verify it exists and user has completed a booking
        if (request.CarId.HasValue)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == request.CarId.Value && !c.IsDeleted, cancellationToken);
            
            if (car == null)
                return Result<Guid>.Failure("Car not found");

            // Check if user has completed a booking for this car
            var hasCompletedBooking = await _context.Bookings
                .AnyAsync(b => b.CarId == request.CarId.Value && 
                              b.UserId == userId && 
                              b.Status == BookingStatus.Completed, cancellationToken);

            if (!hasCompletedBooking)
                return Result<Guid>.Failure("You can only review cars you have completed bookings for");

            // Check if user already reviewed this car
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.CarId == request.CarId.Value && r.ReviewerId == userId, cancellationToken);

            if (existingReview != null)
                return Result<Guid>.Failure("You have already reviewed this car");
        }

        // If reviewing a user, verify they exist (simplified - in production you'd check Users table)
        if (!string.IsNullOrEmpty(request.ReviewedUserId))
        {
            // Cannot review yourself
            if (request.ReviewedUserId == userId)
                return Result<Guid>.Failure("You cannot review yourself");
        }

        var review = new Review
        {
            ReviewerId = userId,
            CarId = request.CarId,
            ReviewedUserId = request.ReviewedUserId,
            Rating = request.Rating,
            Comment = request.Comment,
            IsVerified = request.CarId.HasValue // Auto-verify car reviews if booking exists
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(review.Id);
    }
}
