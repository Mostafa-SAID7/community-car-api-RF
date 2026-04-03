using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Reviews.Queries;

public class GetReviewsQuery : IRequest<Result<List<ReviewDto>>>
{
    public Guid? CarId { get; set; }
    public string? ReviewedUserId { get; set; }
    public string? ReviewerId { get; set; }
    public int? MinRating { get; set; }
    public bool? IsVerified { get; set; }
}
