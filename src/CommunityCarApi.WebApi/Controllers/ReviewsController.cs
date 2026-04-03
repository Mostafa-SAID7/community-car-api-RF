using CommunityCarApi.Application.DTOs;
using CommunityCarApi.Application.Features.Reviews.Commands;
using CommunityCarApi.Application.Features.Reviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCarApi.WebApi.Controllers;

/// <summary>
/// Manages car and user reviews
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all reviews with optional filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetReviews(
        [FromQuery] Guid? carId,
        [FromQuery] string? reviewedUserId,
        [FromQuery] string? reviewerId,
        [FromQuery] int? minRating,
        [FromQuery] bool? isVerified)
    {
        var query = new GetReviewsQuery
        {
            CarId = carId,
            ReviewedUserId = reviewedUserId,
            ReviewerId = reviewerId,
            MinRating = minRating,
            IsVerified = isVerified
        };

        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get review by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(Guid id)
    {
        var query = new GetReviewByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    /// <summary>
    /// Get review statistics for a car or user
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetReviewStatistics(
        [FromQuery] Guid? carId,
        [FromQuery] string? reviewedUserId)
    {
        var query = new GetReviewStatisticsQuery
        {
            CarId = carId,
            ReviewedUserId = reviewedUserId
        };

        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>
    /// Create a new review
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
    {
        var command = new CreateReviewCommand
        {
            CarId = dto.CarId,
            ReviewedUserId = dto.ReviewedUserId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetReviewById), new { id = result.Data }, result.Data)
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Update an existing review
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(Guid id, [FromBody] CreateReviewDto dto)
    {
        var command = new UpdateReviewCommand
        {
            Id = id,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>
    /// Delete a review
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(Guid id)
    {
        var command = new DeleteReviewCommand { Id = id };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
