using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.ExternalRegister;

/// <summary>
/// Command for external provider registration
/// </summary>
public record ExternalRegisterCommand : IRequest<Result<AuthTokenDto>>
{
    public string Provider { get; init; } = string.Empty;
    public string ProviderKey { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string DeviceId { get; init; } = string.Empty;
    public string DeviceType { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
}
