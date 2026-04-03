using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Users.Commands;

public class ChangePasswordCommand : IRequest<Result<bool>>
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
