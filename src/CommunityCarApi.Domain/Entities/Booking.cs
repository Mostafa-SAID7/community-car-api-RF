using CommunityCarApi.Domain.Common;
using CommunityCarApi.Domain.Enums;

namespace CommunityCarApi.Domain.Entities;

public class Booking : BaseEntity
{
    public string BookingNumber { get; set; } = string.Empty;
    public Guid CarId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? Notes { get; set; }
    
    // Navigation properties
    public Car Car { get; set; } = null!;
}
