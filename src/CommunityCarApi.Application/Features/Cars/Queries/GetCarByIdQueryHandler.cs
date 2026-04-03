using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Cars.Queries;

public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, Result<CarDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCarByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CarDto>> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
    {
        var car = await _context.Cars
            .Where(c => c.Id == request.Id && !c.IsDeleted)
            .Select(c => new CarDto
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                Color = c.Color,
                CarType = c.CarType.ToString(),
                FuelType = c.FuelType.ToString(),
                Transmission = c.Transmission.ToString(),
                Seats = c.Seats,
                Description = c.Description,
                HourlyRate = c.HourlyRate,
                DailyRate = c.DailyRate,
                City = c.City,
                State = c.State,
                ImageUrl = c.ImageUrl,
                IsAvailable = c.IsAvailable
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (car == null)
            return Result<CarDto>.Failure("Car not found");

        return Result<CarDto>.Success(car);
    }
}
