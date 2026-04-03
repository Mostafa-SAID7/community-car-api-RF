using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Reviews.Queries;

public class GetReviewStatisticsQuery : IRequest<Result<ReviewStatisticsDto>>
{
    public Guid? CarId { get; set; }
    public string? ReviewedUserId { get; set; }
}
