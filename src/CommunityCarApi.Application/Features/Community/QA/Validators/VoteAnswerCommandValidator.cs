using CommunityCarApi.Application.Features.Community.QA.Commands;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Community.QA.Validators;

public class VoteAnswerCommandValidator : AbstractValidator<VoteAnswerCommand>
{
    public VoteAnswerCommandValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty().WithMessage("AnswerId is required");

        RuleFor(x => x.VoteType)
            .Must(vt => vt == 1 || vt == -1)
            .WithMessage("VoteType must be 1 (upvote) or -1 (downvote)");
    }
}
