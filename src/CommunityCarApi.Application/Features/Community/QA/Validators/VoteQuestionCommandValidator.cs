using CommunityCarApi.Application.Features.Community.QA.Commands;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Community.QA.Validators;

public class VoteQuestionCommandValidator : AbstractValidator<VoteQuestionCommand>
{
    public VoteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("QuestionId is required");

        RuleFor(x => x.VoteType)
            .Must(vt => vt == 1 || vt == -1)
            .WithMessage("VoteType must be 1 (upvote) or -1 (downvote)");
    }
}
