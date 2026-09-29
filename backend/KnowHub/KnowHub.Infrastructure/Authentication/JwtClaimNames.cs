namespace KnowHub.Infrastructure.Authentication;

/// <summary>
/// Claim names as they appear in the token payload. Shared by the generator and
/// the bearer validation setup so the two cannot drift apart.
/// </summary>
public static class JwtClaimNames
{
    public const string Sub = "sub";
    public const string Email = "email";
    public const string Name = "name";
    public const string Role = "role";
    public const string Jti = "jti";
}
