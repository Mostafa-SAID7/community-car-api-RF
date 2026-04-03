using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Cars.Commands;

public class DeleteCarCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
