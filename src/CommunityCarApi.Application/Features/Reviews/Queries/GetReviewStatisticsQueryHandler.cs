using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Reviews.Queries;

public class GetReviewStatisticsQueryHandler : IRequestHandler<GetReviewStatisticsQuery, Result<ReviewStatisticsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ReviewStatisticsDto>> Handle(GetReviewStatisticsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reviews.Where(r => !r.IsDeleted);

        // Apply filters
        if (request.CarId.HasValue)
            query = query.Where(r => r.CarId == request.CarId.Value);

        if (!string.IsNullOrEmpty(request.ReviewedUserId))
            query = query.Where(r => r.ReviewedUserId == request.ReviewedUserId);

        var reviews = await query.ToListAsync(cancellationToken);

        if (!reviews.Any())
        {
            return Result<ReviewStatisticsDto>.Success(new ReviewStatisticsDto
            {
                TotalReviews = 0,
                AverageRating = 0,
                FiveStars = 0,
                FourStars = 0,
                ThreeStars = 0,
                TwoStars = 0,
                OneStar = 0
            });
        }

        var statistics = new ReviewStatisticsDto
        {
            TotalReviews = reviews.Count,
            AverageRating = Math.Round(reviews.Average(r => r.Rating), 2),
            FiveStars = reviews.Count(r => r.Rating == 5),
            FourStars = reviews.Count(r => r.Rating == 4),
            ThreeStars = reviews.Count(r => r.Rating == 3),
            TwoStars = reviews.Count(r => r.Rating == 2),
            OneStar = reviews.Count(r => r.Rating == 1)
        };

        return Result<ReviewStatisticsDto>.Success(statistics);
    }
}
