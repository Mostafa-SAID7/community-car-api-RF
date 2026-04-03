using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AcceptAnswerCommand : IRequest<Result<AnswerDto>>
{
    public Guid AnswerId { get; set; }
}
