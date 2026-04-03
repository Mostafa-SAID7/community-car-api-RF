using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AnswerQuestionCommand : IRequest<Result<AnswerDto>>
{
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = string.Empty;
}
