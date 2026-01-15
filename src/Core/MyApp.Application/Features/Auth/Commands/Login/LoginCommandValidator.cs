using FluentValidation;

namespace MyApp.Application.Features.Auth.Commands.Login;

/// <summary>
/// Validator for LoginCommand
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("Device ID is required");

        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("Device type is required");

        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("Device name is required");
    }
}
