using KnowHub.Domain.Entities;

namespace KnowHub.Application.Interfaces.Repositories;

public interface IUserRepository
{
    /// <summary>Loads a user with roles included, or null. Email match is case-insensitive.</summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
