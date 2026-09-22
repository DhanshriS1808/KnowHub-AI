namespace KnowHub.Domain.Entities;

/// <summary>
/// An account that can sign in and ask questions about documents.
/// </summary>
public class User
{
    public Guid Id { get; set; }

    /// <summary>Login identifier. Stored as citext, so uniqueness ignores casing.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Produced by Identity's PasswordHasher. Never holds a plaintext password.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    /// <summary>Soft-disable switch: false blocks sign-in without deleting history.</summary>
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
