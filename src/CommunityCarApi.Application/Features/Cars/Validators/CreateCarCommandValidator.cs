using CommunityCarApi.Application.Features.Cars.Commands;
using FluentValidation;

namespace CommunityCarApi.Application.Features.Cars.Validators;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(x => x.Make)
            .NotEmpty().WithMessage("Make is required")
            .MaximumLength(50).WithMessage("Make must not exceed 50 characters");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage($"Year must be between 1900 and {DateTime.UtcNow.Year + 1}");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required")
            .MaximumLength(30).WithMessage("Color must not exceed 30 characters");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required")
            .MaximumLength(20).WithMessage("License plate must not exceed 20 characters");

        RuleFor(x => x.CarType)
            .IsInEnum().WithMessage("Invalid car type");

        RuleFor(x => x.FuelType)
            .IsInEnum().WithMessage("Invalid fuel type");

        RuleFor(x => x.Transmission)
            .IsInEnum().WithMessage("Invalid transmission type");

        RuleFor(x => x.Seats)
            .InclusiveBetween(1, 20).WithMessage("Seats must be between 1 and 20");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.HourlyRate)
            .GreaterThan(0).WithMessage("Hourly rate must be greater than 0");

        RuleFor(x => x.DailyRate)
            .GreaterThan(0).WithMessage("Daily rate must be greater than 0");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(100).WithMessage("State must not exceed 100 characters");
    }
}
