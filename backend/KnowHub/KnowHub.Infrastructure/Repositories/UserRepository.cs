using KnowHub.Application.Interfaces.Repositories;
using KnowHub.Domain.Entities;
using KnowHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KnowHub.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly KnowHubDbContext _context;

    public UserRepository(KnowHubDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default) =>
        _context.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default) =>
        await _context.Users.AddAsync(user, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
