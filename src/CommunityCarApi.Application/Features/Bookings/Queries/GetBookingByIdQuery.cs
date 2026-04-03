using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Bookings.Queries;

public class GetBookingByIdQuery : IRequest<Result<BookingDto>>
{
    public Guid Id { get; set; }
}
