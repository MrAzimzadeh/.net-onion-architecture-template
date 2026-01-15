using MediatR;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Roles.Queries.GetAllRoles;

/// <summary>
/// Handler for GetAllRolesQuery
/// </summary>
public class GetAllRolesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllRolesQuery, Result<List<RoleDto>>>
{
    public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await unitOfWork.RoleReads.GetAllAsync(cancellationToken);

        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            var policies = await unitOfWork.RoleReads.GetRolePoliciesAsync(role.Id, cancellationToken);
            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Policies = policies.Select(p => p.Name).ToList()
            });
        }

        return Result.Success(roleDtos, $"Retrieved {roleDtos.Count} roles");
    }
}
