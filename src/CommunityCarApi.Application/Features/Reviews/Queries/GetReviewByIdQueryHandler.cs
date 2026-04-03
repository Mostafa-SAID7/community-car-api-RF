using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Reviews.Queries;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, Result<ReviewDto>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ReviewDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
        
        if (review == null)
            return Result<ReviewDto>.Failure("Review not found");

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            ReviewerId = review.ReviewerId,
            ReviewerName = "User", // Simplified - would need to join with Users table
            CarId = review.CarId,
            ReviewedUserId = review.ReviewedUserId,
            Rating = review.Rating,
            Comment = review.Comment,
            IsVerified = review.IsVerified,
            CreatedAt = review.CreatedAt
        };

        return Result<ReviewDto>.Success(reviewDto);
    }
}
