using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Handler for LogoutCommand
/// </summary>
public class LogoutCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IAuthHelper authHelper) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userId = authHelper.GetUserId(httpContextAccessor.HttpContext);

            // Get device ID from request or from claims
            var deviceId = request.DeviceId ?? authHelper.GetDeviceId(httpContextAccessor.HttpContext);

            if (string.IsNullOrEmpty(deviceId))
                return Result.Failure("Device ID not found");

            // Deactivate device session
            await unitOfWork.UserWrites.DeactivateDeviceSessionAsync(userId, deviceId, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success("Logged out successfully");
        }
        catch (Exception ex)
        {
            return Result.Failure("Failed to logout", ex.Message);
        }
    }
}
