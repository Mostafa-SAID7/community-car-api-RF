using CommunityCarApi.Application.Common;
using CommunityCarApi.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CommunityCarApi.Application.Features.Admin.Users.Commands;

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Result<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssignRoleCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<bool>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<bool>.Failure("User not found", Common.ErrorCodes.UserNotFound);

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);
        if (!roleExists)
            return Result<bool>.Failure("Role does not exist", Common.ErrorCodes.NotFound);

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);
        if (isInRole)
            return Result<bool>.Failure("User already has this role", Common.ErrorCodes.AlreadyExists);

        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        return Result<bool>.Success(true);
    }
}
