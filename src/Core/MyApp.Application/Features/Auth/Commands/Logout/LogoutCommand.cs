using MediatR;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Command to logout from current device
/// </summary>
public record LogoutCommand : IRequest<Result>
{
    public string? DeviceId { get; init; }
}
