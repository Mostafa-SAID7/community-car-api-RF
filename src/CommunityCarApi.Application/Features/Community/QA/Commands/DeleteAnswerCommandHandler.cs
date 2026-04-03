using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteAnswerCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<bool>.Failure("User is not authenticated", ErrorCodes.Unauthorized);
        }

        // Get the answer with its question
        var answer = await _context.Answers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(a => a.Id == request.AnswerId && !a.IsDeleted, cancellationToken);

        if (answer == null)
        {
            return Result<bool>.Failure("Answer not found", ErrorCodes.NotFound);
        }

        // Check if user is the author or an admin
        var isAdmin = _currentUserService.IsInRole("Admin");
        var isAuthor = answer.UserId == userId;

        if (!isAuthor && !isAdmin)
        {
            return Result<bool>.Failure("Only the answer author or an admin can delete this answer", ErrorCodes.Forbidden);
        }

        // Perform soft delete on the answer
        answer.IsDeleted = true;
        answer.DeletedAt = DateTime.UtcNow;

        // If the deleted answer was accepted, set question's IsSolved to false
        if (answer.IsAccepted)
        {
            answer.Question.IsSolved = false;
        }

        // Decrement the question's answer count by 1
        if (answer.Question.AnswerCount > 0)
        {
            answer.Question.AnswerCount--;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
