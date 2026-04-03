using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Reviews.Commands;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteReviewCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (string.IsNullOrEmpty(userId))
            return Result<bool>.Failure("User not authenticated");

        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
        
        if (review == null)
            return Result<bool>.Failure("Review not found");

        // Only the reviewer or admin can delete the review
        if (review.ReviewerId != userId && !isAdmin)
            return Result<bool>.Failure("You can only delete your own reviews");

        review.IsDeleted = true;
        review.DeletedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
