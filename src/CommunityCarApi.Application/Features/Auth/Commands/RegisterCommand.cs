using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Auth;
using MediatR;

namespace CommunityCarApi.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
