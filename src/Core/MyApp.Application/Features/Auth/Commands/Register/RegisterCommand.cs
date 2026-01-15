using MediatR;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Commands.Register;

/// <summary>
/// Command to register a new user
/// </summary>
public record RegisterCommand : IRequest<Result<Guid>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string EmailAddress { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
}
