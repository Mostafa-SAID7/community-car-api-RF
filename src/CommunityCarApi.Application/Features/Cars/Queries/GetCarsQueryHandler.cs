using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Cars.Queries;

public class GetCarsQueryHandler : IRequestHandler<GetCarsQuery, Result<List<CarDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetCarsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CarDto>>> Handle(GetCarsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cars.Where(c => !c.IsDeleted);

        // Apply filters
        if (!string.IsNullOrEmpty(request.City))
            query = query.Where(c => c.City.ToLower().Contains(request.City.ToLower()));

        if (!string.IsNullOrEmpty(request.State))
            query = query.Where(c => c.State.ToLower().Contains(request.State.ToLower()));

        if (request.MinYear.HasValue)
            query = query.Where(c => c.Year >= request.MinYear.Value);

        if (request.MaxYear.HasValue)
            query = query.Where(c => c.Year <= request.MaxYear.Value);

        if (request.MaxDailyRate.HasValue)
            query = query.Where(c => c.DailyRate <= request.MaxDailyRate.Value);

        if (request.CarType.HasValue)
            query = query.Where(c => c.CarType == (CarType)request.CarType.Value);

        if (request.FuelType.HasValue)
            query = query.Where(c => c.FuelType == (FuelType)request.FuelType.Value);

        if (request.Transmission.HasValue)
            query = query.Where(c => c.Transmission == (TransmissionType)request.Transmission.Value);

        if (request.MinSeats.HasValue)
            query = query.Where(c => c.Seats >= request.MinSeats.Value);

        if (request.IsAvailable.HasValue)
            query = query.Where(c => c.IsAvailable == request.IsAvailable.Value);

        var cars = await query
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
            .ToListAsync(cancellationToken);

        return Result<List<CarDto>>.Success(cars);
    }
}
