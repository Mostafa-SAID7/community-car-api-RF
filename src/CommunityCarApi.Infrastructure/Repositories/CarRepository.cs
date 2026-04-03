using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Infrastructure.Repositories;

public class CarRepository : Repository<Car>, ICarRepository
{
    public CarRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Car>> GetAvailableCarsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => !c.IsDeleted && c.IsAvailable)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Car>> SearchCarsAsync(
        string? city, 
        string? state, 
        int? minSeats, 
        decimal? maxPrice, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(c => !c.IsDeleted && c.IsAvailable);

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(c => c.City.ToLower().Contains(city.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(state))
        {
            query = query.Where(c => c.State.ToLower().Contains(state.ToLower()));
        }

        if (minSeats.HasValue)
        {
            query = query.Where(c => c.Seats >= minSeats.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(c => c.DailyRate <= maxPrice.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsLicensePlateUniqueAsync(string licensePlate, Guid? excludeCarId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(c => !c.IsDeleted && c.LicensePlate == licensePlate);

        if (excludeCarId.HasValue)
        {
            query = query.Where(c => c.Id != excludeCarId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }
}
