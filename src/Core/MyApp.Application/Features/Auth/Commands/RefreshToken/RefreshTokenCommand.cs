using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Command to refresh access token using refresh token
/// </summary>
public record RefreshTokenCommand : IRequest<Result<AuthTokenDto>>
{
    public string RefreshToken { get; init; } = string.Empty;
}
