using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Application.Features.Cars.Commands;
using CommunityCarApi.Application.Features.Cars.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCarApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all cars with optional filters
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCars(
        [FromQuery] string? city,
        [FromQuery] string? state,
        [FromQuery] int? minYear,
        [FromQuery] int? maxYear,
        [FromQuery] decimal? maxDailyRate,
        [FromQuery] int? carType,
        [FromQuery] int? fuelType,
        [FromQuery] int? transmission,
        [FromQuery] int? minSeats,
        [FromQuery] bool? isAvailable)
    {
        var query = new GetCarsQuery
        {
            City = city,
            State = state,
            MinYear = minYear,
            MaxYear = maxYear,
            MaxDailyRate = maxDailyRate,
            CarType = carType,
            FuelType = fuelType,
            Transmission = transmission,
            MinSeats = minSeats,
            IsAvailable = isAvailable
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get car by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CarDto>> GetCar(Guid id)
    {
        var query = new GetCarByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Create a new car
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CarDto>> CreateCar(CreateCarCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetCar), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Update an existing car
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<CarDto>> UpdateCar(Guid id, UpdateCarCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete a car (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCar(Guid id)
    {
        var command = new DeleteCarCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return NoContent();
    }
}
