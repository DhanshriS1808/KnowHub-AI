using KnowHub.Domain.Entities;

namespace KnowHub.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    /// <summary>
    /// Issues a signed access token for the user, with one role claim per entry in
    /// <paramref name="roles"/>. Roles are passed in rather than read off the entity,
    /// so the caller controls whether navigation data was loaded.
    /// </summary>
    (string Token, DateTimeOffset ExpiresAt) GenerateToken(User user, IReadOnlyList<string> roles);
}
