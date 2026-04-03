using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetTrendingQuestionsQuery : IRequest<Result<List<QuestionDto>>>
{
    public int Limit { get; set; } = 20;
}
