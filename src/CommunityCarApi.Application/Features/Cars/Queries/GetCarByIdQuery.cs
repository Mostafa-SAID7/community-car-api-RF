using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Cars.Queries;

public class GetCarByIdQuery : IRequest<Result<CarDto>>
{
    public Guid Id { get; set; }
}
