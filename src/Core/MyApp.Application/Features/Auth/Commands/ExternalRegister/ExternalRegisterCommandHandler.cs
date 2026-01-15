using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Auth;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;
using System.Security.Claims;

namespace MyApp.Application.Features.Auth.Commands.ExternalRegister;

/// <summary>
/// Handler for ExternalRegisterCommand
/// </summary>
public class ExternalRegisterCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService) : IRequestHandler<ExternalRegisterCommand, Result<AuthTokenDto>>
{
    private const int RefreshTokenExpirationDays = 30;
    private const int AccessTokenExpirationHours = 1;

    public async Task<Result<AuthTokenDto>> Handle(ExternalRegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists
            var existingUser = await unitOfWork.UserReads.GetByEmailWithRolesAsync(request.Email, cancellationToken);
            if (existingUser != null)
                return Result.Failure<AuthTokenDto>("User with this email already exists");

            // Check if external login already exists
            var existingExternalLogin = await unitOfWork.UserReads.GetByExternalLoginAsync(request.Provider, request.ProviderKey, cancellationToken);
            if (existingExternalLogin != null)
                return Result.Failure<AuthTokenDto>("This external account is already registered");

            // Create new user without password (external auth)
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.Email,
                PhoneNumber = request.PhoneNumber ?? string.Empty,
                PasswordHash = string.Empty,  // No password for external auth
                PasswordSalt = string.Empty,
                IsActive = true,
                CreationAt = DateTime.UtcNow,
                CreatedBy = Guid.Empty
            };

            await unitOfWork.UserWrites.AddAsync(newUser, cancellationToken);

            // Add external login
            var externalLogin = new UserExternalLogin
            {
                Id = Guid.NewGuid(),
                UserId = newUser.Id,
                Provider = request.Provider,


            };
            await unitOfWork.UserWrites.AddExternalLoginAsync(externalLogin, cancellationToken);

            // Create claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                new(ClaimTypes.Name, $"{newUser.FirstName} {newUser.LastName}"),
                new(ClaimTypes.Email, newUser.EmailAddress),
                new("device_id", request.DeviceId),
                new("device_type", request.DeviceType),
                new("auth_provider", request.Provider)
            };

            // Generate tokens
            string accessToken = jwtTokenService.GenerateAccessToken(claims);
            string refreshToken = Guid.NewGuid().ToString("N");

            // Create device session
            var deviceSession = new UserDeviceSession
            {
                Id = Guid.NewGuid(),
                UserId = newUser.Id,
                DeviceId = request.DeviceId,
                DeviceType = request.DeviceType,
                DeviceName = request.DeviceName,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                LastUsedAt = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = newUser.Id
            };
            await unitOfWork.UserWrites.AddDeviceSessionAsync(deviceSession, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new AuthTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddHours(AccessTokenExpirationHours),
                RefreshTokenExpiresAt = deviceSession.RefreshTokenExpiresAt
            }, "Registration successful");
        }
        catch (Exception ex)
        {
            return Result.Failure<AuthTokenDto>("Failed to register with external provider", ex.Message);
        }
    }
}
