namespace KnowHub.Application.DTOs.Auth;

/// <summary>
/// Safe projection of a user. Deliberately has no password field.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public IReadOnlyList<string> Roles { get; set; } = [];
}
