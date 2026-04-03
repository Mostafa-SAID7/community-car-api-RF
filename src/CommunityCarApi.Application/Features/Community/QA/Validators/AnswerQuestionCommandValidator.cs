using CommunityCarApi.Application.Features.Community.QA.Commands;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Community.QA.Validators;

public class AnswerQuestionCommandValidator : AbstractValidator<AnswerQuestionCommand>
{
    public AnswerQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("QuestionId is required");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .Length(20, 5000).WithMessage("Content must be between 20 and 5000 characters");
    }
}
