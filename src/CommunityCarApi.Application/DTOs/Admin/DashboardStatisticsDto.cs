namespace CommunityCarApi.Application.DTOs.Admin;

public class DashboardStatisticsDto
{
    public int TotalUsers { get; set; }
    public int TotalCars { get; set; }
    public int TotalBookings { get; set; }
    public int TotalReviews { get; set; }
    public int ActiveBookings { get; set; }
    public int PendingBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int NewCarsThisMonth { get; set; }
    public double AverageRating { get; set; }
}

public class RealtimeMetricsDto
{
    public int OnlineUsers { get; set; }
    public int ActiveBookings { get; set; }
    public int PendingApprovals { get; set; }
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

public class BusinessMetricsDto
{
    public decimal TotalRevenue { get; set; }
    public decimal AverageBookingValue { get; set; }
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public double BookingCompletionRate { get; set; }
    public List<MonthlyRevenueDto> MonthlyRevenues { get; set; } = new();
    public List<TopCarDto> TopCars { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int BookingCount { get; set; }
}

public class TopCarDto
{
    public Guid CarId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int BookingCount { get; set; }
    public decimal TotalRevenue { get; set; }
}
