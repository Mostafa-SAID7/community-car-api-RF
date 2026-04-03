using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Bookings.Queries;

public class GetBookingsQuery : IRequest<Result<List<BookingDto>>>
{
    public Guid? CarId { get; set; }
    public string? UserId { get; set; }
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
