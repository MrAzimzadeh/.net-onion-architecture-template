using MediatR;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.LogoutAllDevices;

/// <summary>
/// Handler for LogoutAllDevicesCommand
/// </summary>
public class LogoutAllDevicesCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IAuthHelper authHelper) : IRequestHandler<LogoutAllDevicesCommand, Result>
{
    public async Task<Result> Handle(LogoutAllDevicesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get current user ID from claims
            var userId = authHelper.GetUserId(httpContextAccessor.HttpContext);

            // Deactivate all device sessions
            await unitOfWork.UserWrites.DeactivateAllDeviceSessionsAsync(userId, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success("Logged out from all devices successfully");
        }
        catch (Exception ex)
        {
            return Result.Failure("Failed to logout from all devices", ex.Message);
        }
    }
}
