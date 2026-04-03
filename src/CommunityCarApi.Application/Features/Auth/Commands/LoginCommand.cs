using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Auth;
using MediatR;

namespace CommunityCarApi.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
