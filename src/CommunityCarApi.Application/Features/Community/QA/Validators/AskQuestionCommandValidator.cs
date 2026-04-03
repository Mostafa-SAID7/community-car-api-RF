using CommunityCarApi.Application.Features.Community.QA.Commands;
using CommunityCarApi.Domain.Enums;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Community.QA.Validators;

public class AskQuestionCommandValidator : AbstractValidator<AskQuestionCommand>
{
    public AskQuestionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(10, 200).WithMessage("Title must be between 10 and 200 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .Length(20, 5000).WithMessage("Content must be between 20 and 5000 characters");

        RuleFor(x => x.Category)
            .Must(BeValidCategory).WithMessage("Category must be a valid value");

        RuleFor(x => x.Tags)
            .Must(BeValidTags).WithMessage("Each tag must be between 2 and 30 characters")
            .When(x => !string.IsNullOrEmpty(x.Tags));
    }

    private bool BeValidCategory(int category)
    {
        return Enum.IsDefined(typeof(QuestionCategory), category);
    }

    private bool BeValidTags(string? tags)
    {
        if (string.IsNullOrEmpty(tags))
            return true;

        var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        
        return tagList.All(tag => tag.Length >= 2 && tag.Length <= 30);
    }
}
