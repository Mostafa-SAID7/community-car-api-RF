using CommunityCarApi.Application.Common.Interfaces;

namespace CommunityCarApi.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
