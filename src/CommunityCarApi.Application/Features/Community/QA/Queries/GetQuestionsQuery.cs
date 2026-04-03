using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Models;
using CommunityCarApi.Application.DTOs.Community;
using MediatR;

namespace CommunityCarApi.Application.Features.Community.QA.Queries;

public class GetQuestionsQuery : IRequest<Result<PaginatedList<QuestionDto>>>
{
    public int? Category { get; set; }
    public string? SearchTerm { get; set; }
    public string? Tag { get; set; }
    public bool? IsSolved { get; set; }
    public string SortBy { get; set; } = "CreatedAt"; // CreatedAt, VoteCount, AnswerCount, ViewCount
    public bool SortDescending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
