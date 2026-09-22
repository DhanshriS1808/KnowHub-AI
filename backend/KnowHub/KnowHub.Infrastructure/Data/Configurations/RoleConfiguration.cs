using KnowHub.Domain.Entities;
using KnowHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KnowHub.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(r => r.Id);

        // Ids are fixed by RoleType, so they are never database-generated.
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .HasMaxLength(256);

        builder.HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("ix_roles_name");

        builder.HasData(
            new Role
            {
                Id = (int)RoleType.Employee,
                Name = nameof(RoleType.Employee),
                Description = "Can upload documents and ask questions."
            },
            new Role
            {
                Id = (int)RoleType.Admin,
                Name = nameof(RoleType.Admin),
                Description = "Full access, including user management and analytics."
            });
    }
}
