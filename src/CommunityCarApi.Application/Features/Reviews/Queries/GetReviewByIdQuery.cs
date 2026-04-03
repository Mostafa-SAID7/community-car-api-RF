using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Reviews.Queries;

public class GetReviewByIdQuery : IRequest<Result<ReviewDto>>
{
    public Guid Id { get; set; }
}
