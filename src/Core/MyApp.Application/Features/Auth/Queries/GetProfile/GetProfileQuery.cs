using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Auth.Queries.GetProfile;

/// <summary>
/// Query to get current user profile
/// </summary>
public record GetProfileQuery : IRequest<Result<UserDto>>
{
}
