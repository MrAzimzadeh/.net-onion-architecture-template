using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Queries.GetDeviceSessions;

/// <summary>
/// Handler for GetDeviceSessionsQuery
/// </summary>
public class GetDeviceSessionsQueryHandler(IAuthHelper authHelper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetDeviceSessionsQuery, Result<List<DeviceSessionDto>>>
{
    public async Task<Result<List<DeviceSessionDto>>> Handle(GetDeviceSessionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID and device ID from claims
            var userId = authHelper.GetUserId(httpContextAccessor.HttpContext);
            var currentDeviceId = authHelper.GetDeviceId(httpContextAccessor.HttpContext);

            // Get all device sessions for user
            var sessions = await unitOfWork.UserReads.GetUserDeviceSessionsAsync(userId, cancellationToken);

            var sessionDtos = sessions.Select(s => new DeviceSessionDto
            {
                Id = s.Id,
                DeviceId = s.DeviceId ?? string.Empty,
                DeviceType = s.DeviceType ?? string.Empty,
                DeviceName = s.DeviceName ?? string.Empty,
                CreatedAt = s.CreatedAt,
                LastUsedAt = s.LastUsedAt,
                IsActive = s.IsActive,
                IsCurrentDevice = s.DeviceId == currentDeviceId
            }).OrderByDescending(s => s.LastUsedAt).ToList();

            return Result.Success(sessionDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<DeviceSessionDto>>("Failed to get device sessions", ex.Message);
        }
    }
}
