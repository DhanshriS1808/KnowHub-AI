using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KnowHub.Application.Interfaces.Authentication;
using KnowHub.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KnowHub.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public (string Token, DateTimeOffset ExpiresAt) GenerateToken(User user, IReadOnlyList<string> roles)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_settings.AccessTokenMinutes);

        // Short JWT claim names, not the long WS-Federation URIs. The bearer
        // handler is configured with MapInboundClaims = false so these survive
        // validation unchanged.
        var claims = new List<Claim>
        {
            new(JwtClaimNames.Sub, user.Id.ToString()),
            new(JwtClaimNames.Email, user.Email),
            new(JwtClaimNames.Name, user.FullName),
            new(JwtClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // One claim per role, which is what [Authorize(Roles = "Admin")] reads.
        claims.AddRange(roles.Select(role => new Claim(JwtClaimNames.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
