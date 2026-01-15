using MediatR;
using MyApp.Application.Common.Interfaces;
using MyApp.Domain.Common;
using MyApp.Domain.Entities;

namespace MyApp.Application.Features.Roles.Commands.CreateRole;

/// <summary>
/// Handler for CreateRoleCommand
/// </summary>
public class CreateRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if role exists
            var existingRole = await unitOfWork.RoleReads.GetByNameAsync(request.Name, cancellationToken);
            if (existingRole != null)
                return Result.Failure<Guid>("Role with this name already exists");

            // Create role
            var role = new Role
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Empty,
                CreationAt = DateTime.UtcNow,
                Name = request.Name,
                Description = request.Description
            };

            await unitOfWork.RoleWrites.AddAsync(role, cancellationToken);

            return Result.Success(role.Id, "Role created successfully");
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>("Failed to create role", ex.Message);
        }
    }
}
