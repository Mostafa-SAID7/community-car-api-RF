using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Application.Features.Bookings.Commands;
using CommunityCarApi.Application.Features.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCarApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all bookings with optional filters
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings(
        [FromQuery] Guid? carId,
        [FromQuery] string? userId,
        [FromQuery] int? status,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var query = new GetBookingsQuery
        {
            CarId = carId,
            UserId = userId,
            Status = status,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get booking by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(Guid id)
    {
        var query = new GetBookingByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Create a new booking
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetBooking), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Cancel a booking
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        var command = new CancelBookingCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
