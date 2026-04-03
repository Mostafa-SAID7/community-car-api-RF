using CommunityCarApi.Application.Features.Community.QA.Commands;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Community.QA.Validators;

public class AcceptAnswerCommandValidator : AbstractValidator<AcceptAnswerCommand>
{
    public AcceptAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty().WithMessage("AnswerId is required");
    }
}
