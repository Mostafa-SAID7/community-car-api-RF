using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Cars.Queries;

public class GetCarsQuery : IRequest<Result<List<CarDto>>>
{
    public string? City { get; set; }
    public string? State { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public decimal? MaxDailyRate { get; set; }
    public int? CarType { get; set; }
    public int? FuelType { get; set; }
    public int? Transmission { get; set; }
    public int? MinSeats { get; set; }
    public bool? IsAvailable { get; set; }
}
