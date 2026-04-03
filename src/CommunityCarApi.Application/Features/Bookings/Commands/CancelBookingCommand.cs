using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Bookings.Commands;

public class CancelBookingCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
