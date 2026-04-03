using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetUserReputationQuery : IRequest<Result<UserReputationDto>>
{
}
