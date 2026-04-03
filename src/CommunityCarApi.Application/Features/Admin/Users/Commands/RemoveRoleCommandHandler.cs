using CommunityCarApi.Application.Common;
using CommunityCarApi.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CommunityCarApi.Application.Features.Admin.Users.Commands;

public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RemoveRoleCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<bool>.Failure("User not found", Common.ErrorCodes.UserNotFound);

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);
        if (!isInRole)
            return Result<bool>.Failure("User does not have this role", Common.ErrorCodes.NotFound);

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
        if (!result.Succeeded)
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        return Result<bool>.Success(true);
    }
}
