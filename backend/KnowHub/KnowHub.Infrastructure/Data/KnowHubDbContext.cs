using KnowHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowHub.Infrastructure.Data;

/// <summary>
/// EF Core context for the KnowHub relational store (PostgreSQL).
/// </summary>
public class KnowHubDbContext : DbContext
{
    public KnowHubDbContext(DbContextOptions<KnowHubDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Required by the case-insensitive email column on users.
        modelBuilder.HasPostgresExtension("citext");

        // Picks up every IEntityTypeConfiguration<> under Data/Configurations.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KnowHubDbContext).Assembly);
    }
}
