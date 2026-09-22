namespace KnowHub.Domain.Entities;

/// <summary>
/// Join entity assigning a <see cref="Role"/> to a <see cref="User"/>.
/// </summary>
public class UserRole
{
    public Guid UserId { get; set; }

    public int RoleId { get; set; }

    public DateTimeOffset AssignedAt { get; set; }

    public User User { get; set; } = null!;

    public Role Role { get; set; } = null!;
}
