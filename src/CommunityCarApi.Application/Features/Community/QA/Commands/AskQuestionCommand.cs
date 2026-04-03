using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class AskQuestionCommand : IRequest<Result<QuestionDto>>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Category { get; set; }
    public string? Tags { get; set; }
}
