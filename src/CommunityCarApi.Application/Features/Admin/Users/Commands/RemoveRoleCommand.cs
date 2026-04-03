using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Admin.Users.Commands;

public class RemoveRoleCommand : IRequest<Result<bool>>
{
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
