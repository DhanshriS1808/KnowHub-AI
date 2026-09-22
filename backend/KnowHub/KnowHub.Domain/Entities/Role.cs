namespace KnowHub.Domain.Entities;

/// <summary>
/// A named permission group. Seeded with Employee and Admin.
/// </summary>
public class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
