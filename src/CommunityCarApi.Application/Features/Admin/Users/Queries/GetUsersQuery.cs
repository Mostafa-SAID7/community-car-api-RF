using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Admin;
using MediatR;

namespace CommunityCarApi.Application.Features.Admin.Users.Queries;

public class GetUsersQuery : IRequest<Result<List<AdminUserDto>>>
{
    public string? SearchTerm { get; set; }
    public string? Role { get; set; }
    public bool? IsEmailVerified { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
