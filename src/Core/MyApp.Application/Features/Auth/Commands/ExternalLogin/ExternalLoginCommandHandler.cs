using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System.Security.Claims;

namespace MyApp.Application.Features.Auth.Commands.ExternalLogin;

/// <summary>
/// Handler for ExternalLoginCommand
/// </summary>
public class ExternalLoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService) : IRequestHandler<ExternalLoginCommand, Result<AuthTokenDto>>
{
    private const int RefreshTokenExpirationDays = 30;
    private const int AccessTokenExpirationHours = 1;

    public async Task<Result<AuthTokenDto>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user exists with this external login
            var user = await unitOfWork.UserReads.GetByExternalLoginAsync(request.Provider, request.ProviderKey, cancellationToken);

            if (user == null)
            {
                // Try to find by email
                user = await unitOfWork.UserReads.GetByEmailWithRolesAsync(request.Email, cancellationToken);

                if (user == null)
                {
                    return Result.Failure<AuthTokenDto>("User not found. Please register first.");
                }

                // Link external login to existing user
                var externalLogin = new UserExternalLogin
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Provider = request.Provider,
                    CreatedAt = DateTime.UtcNow,

                };
                await unitOfWork.UserWrites.AddExternalLoginAsync(externalLogin, cancellationToken);
            }

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
                new("device_id", request.DeviceId),
                new("device_type", request.DeviceType),
                new("auth_provider", request.Provider)
            };

            if (user.TenantId.HasValue)
            {
                claims.Add(new Claim("tenant_id", user.TenantId.Value.ToString()));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            // Generate tokens
            string accessToken = jwtTokenService.GenerateAccessToken(claims);
            string refreshToken = Guid.NewGuid().ToString("N");

            // Find or create device session
            var deviceSession = await unitOfWork.UserReads.GetDeviceSessionAsync(user.Id, request.DeviceId, cancellationToken);

            if (deviceSession == null)
            {
                deviceSession = new UserDeviceSession
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    DeviceId = request.DeviceId,
                    DeviceType = request.DeviceType,
                    DeviceName = request.DeviceName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = user.Id
                };
                await unitOfWork.UserWrites.AddDeviceSessionAsync(deviceSession, cancellationToken);
            }

            // Update session
            deviceSession.RefreshToken = refreshToken;
            deviceSession.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays);
            deviceSession.LastUsedAt = DateTime.UtcNow;
            deviceSession.IsActive = true;

            await unitOfWork.UserWrites.UpdateDeviceSessionAsync(deviceSession, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new AuthTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddHours(AccessTokenExpirationHours),
                RefreshTokenExpiresAt = deviceSession.RefreshTokenExpiresAt
            }, "External login successful");
        }
        catch (Exception ex)
        {
            return Result.Failure<AuthTokenDto>("Failed to login with external provider", ex.Message);
        }
    }
}
