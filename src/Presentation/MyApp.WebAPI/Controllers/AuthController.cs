using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Features.Auth.Commands.ChangePassword;
using MyApp.Application.Features.Auth.Commands.ExternalLogin;
using MyApp.Application.Features.Auth.Commands.ExternalRegister;
using MyApp.Application.Features.Auth.Commands.Login;
using MyApp.Application.Features.Auth.Commands.Logout;
using MyApp.Application.Features.Auth.Commands.LogoutAllDevices;
using MyApp.Application.Features.Auth.Commands.RefreshToken;
using MyApp.Application.Features.Auth.Commands.Register;
using MyApp.Application.Features.Auth.Queries.GetDeviceSessions;
using MyApp.Application.Features.Auth.Queries.GetProfile;

namespace MyApp.WebAPI.Controllers;

/// <summary>
/// Authentication Controller - Login, Register, Profile Management
/// </summary>
public class AuthController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Login user with email and password
    /// </summary>
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    [HttpPut("refresh-sign-in/{refreshToken}")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshSignIn(string refreshToken)
    {
        var result = await _mediator.Send(new RefreshTokenCommand { RefreshToken = refreshToken });

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }

    /// <summary>
    /// Login with external provider (Google, Facebook, etc.)
    /// </summary>
    [HttpPost("external-login")]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Register with external provider (Google, Facebook, etc.)
    /// </summary>
    [HttpPost("external-register")]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalRegister([FromBody] ExternalRegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _mediator.Send(new GetProfileQuery());

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Logout from current device
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand? command = null)
    {
        var result = await _mediator.Send(command ?? new LogoutCommand());

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Logout from all devices
    /// </summary>
    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAllDevices()
    {
        var result = await _mediator.Send(new LogoutAllDevicesCommand());

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get all active device sessions
    /// </summary>
    [HttpGet("device-sessions")]
    [Authorize]
    public async Task<IActionResult> GetDeviceSessions()
    {
        var result = await _mediator.Send(new GetDeviceSessionsQuery());

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get current user info (legacy endpoint - use /profile instead)
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst("uid")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Roles = roles
        });
    }
}
