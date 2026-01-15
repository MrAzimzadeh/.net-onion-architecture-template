using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;
using System.Security.Claims;

namespace MyApp.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Handler for RefreshTokenCommand
/// </summary>
public class RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService) : IRequestHandler<RefreshTokenCommand, Result<AuthTokenDto>>
{
    private const int RefreshTokenExpirationDays = 30;
    private const int AccessTokenExpirationHours = 1;

    public async Task<Result<AuthTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get device session by refresh token
            var deviceSession = await unitOfWork.UserReads.GetDeviceSessionByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (deviceSession == null || !deviceSession.IsActive)
                return Result.Failure<AuthTokenDto>("Invalid or expired refresh token");

            if (deviceSession.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return Result.Failure<AuthTokenDto>("Refresh token has expired");

            // Get user with roles
            var user = await unitOfWork.UserReads.GetByIdWithRolesAsync(deviceSession.UserId, cancellationToken);

            if (user == null)
                return Result.Failure<AuthTokenDto>("User not found");

            if (!user.IsActive)
                return Result.Failure<AuthTokenDto>("User account is deactivated");

            // Get user roles
            var roles = await unitOfWork.UserReads.GetUserRolesAsync(user.Id, cancellationToken);

            // Create claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Email, user.EmailAddress),
                new("device_id", deviceSession.DeviceId ?? string.Empty),
                new("device_type", deviceSession.DeviceType ?? string.Empty)
            };

            if (user.TenantId.HasValue)
            {
                claims.Add(new Claim("tenant_id", user.TenantId.Value.ToString()));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            // Generate new tokens
            string accessToken = jwtTokenService.GenerateAccessToken(claims);
            string newRefreshToken = Guid.NewGuid().ToString("N");

            // Rotate refresh token
            deviceSession.RefreshToken = newRefreshToken;
            deviceSession.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays);
            deviceSession.LastUsedAt = DateTime.UtcNow;

            await unitOfWork.UserWrites.UpdateDeviceSessionAsync(deviceSession, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new AuthTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddHours(AccessTokenExpirationHours),
                RefreshTokenExpiresAt = deviceSession.RefreshTokenExpiresAt
            }, "Token refreshed successfully");
        }
        catch (Exception ex)
        {
            return Result.Failure<AuthTokenDto>("Failed to refresh token", ex.Message);
        }
    }
}
