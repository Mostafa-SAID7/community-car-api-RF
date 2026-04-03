using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Cars.Commands;

public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, Result<CarDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCarCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CarDto>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
        var car = await _context.Cars
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (car == null)
            return Result<CarDto>.Failure("Car not found");

        // Check ownership
        var userId = _currentUserService.UserId;
        if (car.OwnerId != userId && !_currentUserService.IsInRole("Admin"))
            return Result<CarDto>.Failure("You don't have permission to update this car");

        car.Make = request.Make;
        car.Model = request.Model;
        car.Year = request.Year;
        car.Color = request.Color;
        car.CarType = (CarType)request.CarType;
        car.FuelType = (FuelType)request.FuelType;
        car.Transmission = (TransmissionType)request.Transmission;
        car.Seats = request.Seats;
        car.Description = request.Description;
        car.HourlyRate = request.HourlyRate;
        car.DailyRate = request.DailyRate;
        car.City = request.City;
        car.State = request.State;
        car.IsAvailable = request.IsAvailable;

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
