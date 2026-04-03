using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Cars.Commands;

public class CreateCarCommand : IRequest<Result<CarDto>>
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public int CarType { get; set; }
    public int FuelType { get; set; }
    public int Transmission { get; set; }
    public int Seats { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public decimal DailyRate { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
