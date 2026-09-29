using KnowHub.Application.Interfaces.Authentication;
using KnowHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KnowHub.Infrastructure.Authentication;

/// <summary>
/// Wraps ASP.NET Core Identity's PasswordHasher, which applies PBKDF2 with a
/// per-password salt and embeds the iteration count in the stored string.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _inner = new();

    public string Hash(string password) => _inner.HashPassword(null!, password);

    public bool Verify(string hash, string password)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return false;
        }

        var result = _inner.VerifyHashedPassword(null!, hash, password);

        return result is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
