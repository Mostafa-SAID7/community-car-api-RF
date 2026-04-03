using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Models;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetMyQuestionsQuery : IRequest<Result<PaginatedList<QuestionDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
