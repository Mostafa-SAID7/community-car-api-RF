using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class VoteQuestionCommand : IRequest<Result<VoteResultDto>>
{
    public Guid QuestionId { get; set; }
    public int VoteType { get; set; } // 1 for upvote, -1 for downvote
}
