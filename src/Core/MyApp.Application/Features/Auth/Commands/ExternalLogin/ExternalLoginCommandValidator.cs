using FluentValidation;

namespace MyApp.Application.Features.Auth.Commands.ExternalLogin;

/// <summary>
/// Validator for ExternalLoginCommand
/// </summary>
public class ExternalLoginCommandValidator : AbstractValidator<ExternalLoginCommand>
{
    public ExternalLoginCommandValidator()
    {
        RuleFor(x => x.Provider)
            .NotEmpty().WithMessage("Provider is required");

        RuleFor(x => x.ProviderKey)
            .NotEmpty().WithMessage("Provider key is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("Device ID is required");

        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("Device type is required");

        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("Device name is required");
    }
}
