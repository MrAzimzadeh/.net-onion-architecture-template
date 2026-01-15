using MediatR;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.ChangePassword;

/// <summary>
/// Command to change user password
/// </summary>
public record ChangePasswordCommand : IRequest<Result>
{
    public string CurrentPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
}
