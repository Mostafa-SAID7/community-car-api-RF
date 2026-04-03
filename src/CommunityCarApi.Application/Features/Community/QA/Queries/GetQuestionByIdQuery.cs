using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetQuestionByIdQuery : IRequest<Result<QuestionDetailDto>>
{
    public Guid QuestionId { get; set; }
}
