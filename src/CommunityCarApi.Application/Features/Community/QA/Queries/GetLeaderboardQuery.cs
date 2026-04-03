using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Models;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetLeaderboardQuery : IRequest<Result<PaginatedList<LeaderboardEntryDto>>>
{
    public string TimePeriod { get; set; } = "AllTime"; // AllTime, Monthly, Weekly
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
