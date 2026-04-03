using CommunityCarApi.Application.DTOs.Community;
using CommunityCarApi.Application.Features.Community.QA.Commands;
using CommunityCarApi.Application.Features.Community.QA.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCarApi.WebApi.Controllers;

[ApiController]
[Route("api/qa")]
public class QAController : ControllerBase
{
    private readonly IMediator _mediator;

    public QAController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Ask a question
    /// </summary>
    [HttpPost("questions")]
    [Authorize]
    public async Task<ActionResult<QuestionDto>> AskQuestion(AskQuestionCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetQuestion), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Get trending questions
    /// </summary>
    [HttpGet("questions/trending")]
    public async Task<IActionResult> GetTrendingQuestions([FromQuery] int limit = 20)
    {
        var query = new GetTrendingQuestionsQuery { Limit = limit };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get user's questions
    /// </summary>
    [HttpGet("questions/my")]
    [Authorize]
    public async Task<IActionResult> GetMyQuestions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetMyQuestionsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get questions with filters
    /// </summary>
    [HttpGet("questions")]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] int? category,
        [FromQuery] string? searchTerm,
        [FromQuery] string? tag,
        [FromQuery] bool? isSolved,
        [FromQuery] string sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetQuestionsQuery
        {
            Category = category,
            SearchTerm = searchTerm,
            Tag = tag,
            IsSolved = isSolved,
            SortBy = sortBy,
            SortDescending = sortDescending,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get question details
    /// </summary>
    [HttpGet("questions/{id}")]
    public async Task<ActionResult<QuestionDetailDto>> GetQuestion(Guid id)
    {
        var query = new GetQuestionByIdQuery { QuestionId = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete question
    /// </summary>
    [HttpDelete("questions/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var command = new DeleteQuestionCommand { QuestionId = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return NoContent();
    }

    /// <summary>
    /// Answer a question
    /// </summary>
    [HttpPost("answers")]
    [Authorize]
    public async Task<ActionResult<AnswerDto>> AnswerQuestion(AnswerQuestionCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Delete answer
    /// </summary>
    [HttpDelete("answers/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAnswer(Guid id)
    {
        var command = new DeleteAnswerCommand { AnswerId = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return NoContent();
    }

    /// <summary>
    /// Vote on question
    /// </summary>
    [HttpPost("questions/{id}/vote")]
    [Authorize]
    public async Task<ActionResult<VoteResultDto>> VoteOnQuestion(Guid id, [FromBody] VoteQuestionCommand command)
    {
        command.QuestionId = id;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Vote on answer
    /// </summary>
    [HttpPost("answers/{id}/vote")]
    [Authorize]
    public async Task<ActionResult<VoteResultDto>> VoteOnAnswer(Guid id, [FromBody] VoteAnswerCommand command)
    {
        command.AnswerId = id;
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Accept answer
    /// </summary>
    [HttpPost("answers/{id}/accept")]
    [Authorize]
    public async Task<ActionResult<AnswerDto>> AcceptAnswer(Guid id)
    {
        var command = new AcceptAnswerCommand { AnswerId = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get leaderboard
    /// </summary>
    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard(
        [FromQuery] string timePeriod = "AllTime",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetLeaderboardQuery
        {
            TimePeriod = timePeriod,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get current user's reputation
    /// </summary>
    [HttpGet("reputation")]
    [Authorize]
    public async Task<ActionResult<UserReputationDto>> GetReputation()
    {
        var query = new GetUserReputationQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
