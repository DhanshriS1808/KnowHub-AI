using KnowHub.Application.DTOs.Auth;
using KnowHub.Application.Interfaces.Services;
using KnowHub.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Creates an account and returns a token. New accounts get the Employee role.</summary>
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _authService.RegisterAsync(request, cancellationToken));
    }

    /// <summary>Exchanges email and password for an access token.</summary>
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _authService.LoginAsync(request, cancellationToken));
    }

    /// <summary>Echoes the claims in the bearer token. Useful for verifying auth works.</summary>
    [HttpGet("me")]
    [Authorize]
    //[ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserDto> Me()
    {
        var id = User.FindFirst(JwtClaimNames.Sub)?.Value;

        return Ok(new UserDto
        {
            Id = Guid.TryParse(id, out var parsed) ? parsed : Guid.Empty,
            Email = User.FindFirst(JwtClaimNames.Email)?.Value ?? string.Empty,
            FullName = User.FindFirst(JwtClaimNames.Name)?.Value ?? string.Empty,
            Roles = User.FindAll(JwtClaimNames.Role).Select(c => c.Value).ToArray()
        });
    }
}
