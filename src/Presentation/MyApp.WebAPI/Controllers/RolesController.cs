using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Features.Roles.Commands.CreateRole;
using MyApp.Application.Features.Roles.Queries.GetAllRoles;
using MyApp.Application.Security;
using MyApp.Infrastructure.Authentication;

namespace MyApp.WebAPI.Controllers;

/// <summary>
/// Roles Controller - Role management
/// </summary>
public class RolesController(IMediator mediator) : BaseController
{

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    [Policy(Policies.Users.List)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllRolesQuery();
        var result = await mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Create new role
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
