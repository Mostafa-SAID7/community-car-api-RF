using CommunityCarApi.Application.Features.Reviews.Commands;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Reviews.Validators;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Review ID is required");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment is required")
            .MaximumLength(1000)
            .WithMessage("Comment cannot exceed 1000 characters");
    }
}
