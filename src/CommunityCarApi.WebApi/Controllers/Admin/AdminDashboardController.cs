using CommunityCarApi.Application.Features.Admin.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCarApi.WebApi.Controllers.Admin;

/// <summary>
/// Admin dashboard statistics and metrics
/// </summary>
[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin")]
public class AdminDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var query = new GetDashboardStatisticsQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get business metrics
    /// </summary>
    [HttpGet("metrics")]
    public async Task<IActionResult> GetBusinessMetrics([FromQuery] int monthsBack = 6)
    {
        var query = new GetBusinessMetricsQuery { MonthsBack = monthsBack };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }
}
