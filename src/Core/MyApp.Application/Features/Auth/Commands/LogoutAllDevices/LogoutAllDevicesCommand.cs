using MediatR;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.LogoutAllDevices;

/// <summary>
/// Command to logout from all devices
/// </summary>
public record LogoutAllDevicesCommand : IRequest<Result>
{
}
