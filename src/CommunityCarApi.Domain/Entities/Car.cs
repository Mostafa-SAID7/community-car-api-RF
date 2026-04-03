using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Enums;

namespace CommunityCarApi.Domain.Entities;

public class Car : BaseEntity
{
    public string OwnerId { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public CarType CarType { get; set; }
    public FuelType FuelType { get; set; }
    public TransmissionType Transmission { get; set; }
    public int Seats { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public decimal DailyRate { get; set; }
    public int Mileage { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; } = true;
    
    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
