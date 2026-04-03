using CommunityCarApi.Domain.Entities;

namespace CommunityCarApi.Infrastructure.Repositories;

public interface ICarRepository : IRepository<Car>
{
    Task<IEnumerable<Car>> GetAvailableCarsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Car>> SearchCarsAsync(string? city, string? state, int? minSeats, decimal? maxPrice, CancellationToken cancellationToken = default);
    Task<bool> IsLicensePlateUniqueAsync(string licensePlate, Guid? excludeCarId = null, CancellationToken cancellationToken = default);
}
