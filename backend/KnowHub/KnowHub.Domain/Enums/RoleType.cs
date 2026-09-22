namespace KnowHub.Domain.Enums;

/// <summary>
/// Well-known role ids. Values match the seeded rows in the roles table,
/// so they can be used directly when assigning roles or checking claims.
/// </summary>
public enum RoleType
{
    Employee = 1,
    Admin = 2
}
