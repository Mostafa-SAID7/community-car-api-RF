using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Reviews.Commands;

public class CreateReviewCommand : IRequest<Result<Guid>>
{
    public Guid? CarId { get; set; }
    public string? ReviewedUserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
