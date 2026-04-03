using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs;
using MediatR;

namespace CommunityCarApi.Application.Features.Users.Queries;

public class GetUserStatisticsQuery : IRequest<Result<UserStatisticsDto>>
{
}
