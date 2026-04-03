using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class DeleteQuestionCommand : IRequest<Result<bool>>
{
    public Guid QuestionId { get; set; }
}
