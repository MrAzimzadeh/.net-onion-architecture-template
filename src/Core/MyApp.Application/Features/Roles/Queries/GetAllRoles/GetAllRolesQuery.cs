using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Roles.Queries.GetAllRoles;

/// <summary>
/// Query to get all roles
/// </summary>
public record GetAllRolesQuery : IRequest<Result<List<RoleDto>>>;
