using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Domain.Enums;
using MediatR;

namespace CommunityCarApi.Application.Features.Cars.Commands;

public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Result<CarDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateCarCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CarDto>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? "system";

        var car = new Car
        {
            OwnerId = userId,
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            Color = request.Color,
            LicensePlate = request.LicensePlate,
            CarType = (CarType)request.CarType,
            FuelType = (FuelType)request.FuelType,
            Transmission = (TransmissionType)request.Transmission,
            Seats = request.Seats,
            Description = request.Description,
            HourlyRate = request.HourlyRate,
            DailyRate = request.DailyRate,
            City = request.City,
            State = request.State,
            Mileage = 0,
            IsAvailable = true
        };

        _context.Cars.Add(car);
        await _context.SaveChangesAsync(cancellationToken);

        var carDto = new CarDto
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Color = car.Color,
            CarType = car.CarType.ToString(),
            FuelType = car.FuelType.ToString(),
            Transmission = car.Transmission.ToString(),
            Seats = car.Seats,
            Description = car.Description,
            HourlyRate = car.HourlyRate,
            DailyRate = car.DailyRate,
            City = car.City,
            State = car.State,
            ImageUrl = car.ImageUrl,
            IsAvailable = car.IsAvailable
        };

        return Result<CarDto>.Success(carDto);
    }
}
