using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System.Security.Claims;

namespace MyApp.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handler for LoginCommand
/// </summary>
public class LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<AuthTokenDto>>
{
    private const int RefreshTokenExpirationDays = 30;
    private const int AccessTokenExpirationHours = 1;

    public async Task<Result<AuthTokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user by email with roles
            var user = await unitOfWork.UserReads.GetByEmailWithRolesAsync(request.EmailAddress, cancellationToken);
            
            if (user == null)
                return Result.Failure<AuthTokenDto>("Invalid email or password");

            // Check if user is active
            if (!user.IsActive)
                return Result.Failure<AuthTokenDto>("User account is deactivated");

            // Verify password
            if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                return Result.Failure<AuthTokenDto>("Invalid email or password");

            // Get user roles
            var roles = await unitOfWork.UserReads.GetUserRolesAsync(user.Id, cancellationToken);

            // Create claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Email, user.EmailAddress),
                new("device_id", request.DeviceId),
                new("device_type", request.DeviceType)
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
            }, "Login successful");
        }
        catch (Exception ex)
        {
            return Result.Failure<AuthTokenDto>("Failed to login", ex.Message);
        }
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
            return false;

        try
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new System.Security.Cryptography.HMACSHA512(saltBytes);
            var hashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == storedHash;
        }
        catch
        {
            return false;
        }
    }
}
