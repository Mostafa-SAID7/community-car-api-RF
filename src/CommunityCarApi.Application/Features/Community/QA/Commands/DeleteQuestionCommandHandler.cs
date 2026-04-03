using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Community.QA.Commands;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteQuestionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<bool>.Failure("User is not authenticated", ErrorCodes.Unauthorized);
        }

        // Get the question
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId && !q.IsDeleted, cancellationToken);

        if (question == null)
        {
            return Result<bool>.Failure("Question not found", ErrorCodes.NotFound);
        }

        // Check if user is the author or an admin
        var isAdmin = _currentUserService.IsInRole("Admin");
        var isAuthor = question.UserId == userId;

        if (!isAuthor && !isAdmin)
        {
            return Result<bool>.Failure("Only the question author or an admin can delete this question", ErrorCodes.Forbidden);
        }

        // Perform soft delete on the question
        question.IsDeleted = true;
        question.DeletedAt = DateTime.UtcNow;

        // Soft delete all associated answers
        var answers = await _context.Answers
            .Where(a => a.QuestionId == request.QuestionId && !a.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var answer in answers)
        {
            answer.IsDeleted = true;
            answer.DeletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
