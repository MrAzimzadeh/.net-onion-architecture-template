using FluentValidation;

namespace MyApp.Application.Features.Auth.Commands.ExternalRegister;

/// <summary>
/// Validator for ExternalRegisterCommand
/// </summary>
public class ExternalRegisterCommandValidator : AbstractValidator<ExternalRegisterCommand>
{
    public ExternalRegisterCommandValidator()
    {
        RuleFor(x => x.Provider)
            .NotEmpty().WithMessage("Provider is required");

        RuleFor(x => x.ProviderKey)
            .NotEmpty().WithMessage("Provider key is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("Device ID is required");

        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("Device type is required");

        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("Device name is required");
    }
}
