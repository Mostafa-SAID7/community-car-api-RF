using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Reviews.Commands;

public class UpdateReviewCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
