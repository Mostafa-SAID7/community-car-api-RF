using CommunityCarApi.Application.Common;
using CommunityCarApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityCarApi.Application.Features.Cars.Commands;

public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public DeleteCarCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<Result<bool>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
    {
        var car = await _context.Cars
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (car == null)
            return Result<bool>.Failure("Car not found");

        // Check ownership
        var userId = _currentUserService.UserId;
        if (car.OwnerId != userId && !_currentUserService.IsInRole("Admin"))
            return Result<bool>.Failure("You don't have permission to delete this car");

        car.IsDeleted = true;
        car.DeletedAt = _dateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
