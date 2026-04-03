using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Reviews.Commands;

public class DeleteReviewCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
