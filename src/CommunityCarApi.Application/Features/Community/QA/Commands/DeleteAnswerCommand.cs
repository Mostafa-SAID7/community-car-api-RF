using CommunityCarApi.Application.Common;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class DeleteAnswerCommand : IRequest<Result<bool>>
{
    public Guid AnswerId { get; set; }
}
