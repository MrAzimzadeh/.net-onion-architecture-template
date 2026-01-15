using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Queries.GetDeviceSessions;

/// <summary>
/// Query to get all device sessions for current user
/// </summary>
public record GetDeviceSessionsQuery : IRequest<Result<List<DeviceSessionDto>>>
{
}
