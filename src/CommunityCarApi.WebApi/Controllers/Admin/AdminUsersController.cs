using CommunityCarApi.Application.Features.Admin.Users.Commands;
using CommunityCarApi.Application.Features.Admin.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCarApi.WebApi.Controllers.Admin;

/// <summary>
/// Admin user management
/// </summary>
[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users with filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? searchTerm,
        [FromQuery] string? role,
        [FromQuery] bool? isEmailVerified,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetUsersQuery
        {
            SearchTerm = searchTerm,
            Role = role,
            IsEmailVerified = isEmailVerified,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>
    /// Assign role to user
    /// </summary>
    [HttpPost("{userId}/roles")]
    public async Task<IActionResult> AssignRole(string userId, [FromBody] AssignRoleRequest request)
    {
        var command = new AssignRoleCommand
        {
            UserId = userId,
            Role = request.Role
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>
    /// Remove role from user
    /// </summary>
    [HttpDelete("{userId}/roles/{role}")]
    public async Task<IActionResult> RemoveRole(string userId, string role)
    {
        var command = new RemoveRoleCommand
        {
            UserId = userId,
            Role = role
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}

public class AssignRoleRequest
{
    public string Role { get; set; } = string.Empty;
}
