using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Reviews.Queries;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, Result<List<ReviewDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ReviewDto>>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reviews.Where(r => !r.IsDeleted);

        // Apply filters
        if (request.CarId.HasValue)
            query = query.Where(r => r.CarId == request.CarId.Value);

        if (!string.IsNullOrEmpty(request.ReviewedUserId))
            query = query.Where(r => r.ReviewedUserId == request.ReviewedUserId);

        if (!string.IsNullOrEmpty(request.ReviewerId))
            query = query.Where(r => r.ReviewerId == request.ReviewerId);

        if (request.MinRating.HasValue)
            query = query.Where(r => r.Rating >= request.MinRating.Value);

        if (request.IsVerified.HasValue)
            query = query.Where(r => r.IsVerified == request.IsVerified.Value);

        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);

        var reviewDtos = reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            ReviewerId = r.ReviewerId,
            ReviewerName = "User", // Simplified - would need to join with Users table
            CarId = r.CarId,
            ReviewedUserId = r.ReviewedUserId,
            Rating = r.Rating,
            Comment = r.Comment,
            IsVerified = r.IsVerified,
            CreatedAt = r.CreatedAt
        }).ToList();

        return Result<List<ReviewDto>>.Success(reviewDtos);
    }
}
