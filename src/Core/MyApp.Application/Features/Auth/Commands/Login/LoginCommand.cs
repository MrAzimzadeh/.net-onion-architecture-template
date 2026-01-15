using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.Login;

/// <summary>
/// Command to login user with device information
/// </summary>
public record LoginCommand : IRequest<Result<AuthTokenDto>>
{
    public string EmailAddress { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string DeviceId { get; init; } = string.Empty;
    public string DeviceType { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
}
