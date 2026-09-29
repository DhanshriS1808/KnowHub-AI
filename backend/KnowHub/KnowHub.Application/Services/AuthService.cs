using KnowHub.Application.DTOs.Auth;
using KnowHub.Application.Exceptions;
using KnowHub.Application.Interfaces.Authentication;
using KnowHub.Application.Interfaces.Repositories;
using KnowHub.Application.Interfaces.Services;
using KnowHub.Domain.Entities;
using KnowHub.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace KnowHub.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator,
        ILogger<AuthService> logger)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = Normalize(request.Email);

        if (await _users.EmailExistsAsync(email, cancellationToken))
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = request.FullName.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Every self-registered account starts as an Employee. Admin is granted
        // deliberately, never picked by the caller.
        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = (int)RoleType.Employee,
            AssignedAt = DateTimeOffset.UtcNow
        });

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Registered new user {UserId}", user.Id);

        return BuildResponse(user, [nameof(RoleType.Employee)]);
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByEmailAsync(Normalize(request.Email), cancellationToken);

        // Verify against a throwaway hash even when the user is missing, so the
        // response time does not reveal whether the email is registered.
        var storedHash = user?.PasswordHash ?? string.Empty;
        var passwordValid = _passwordHasher.Verify(storedHash, request.Password);

        if (user is null || !passwordValid || !user.IsActive)
        {
            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
            throw new AuthenticationFailedException();
        }

        user.LastLoginAt = DateTimeOffset.UtcNow;
        await _users.SaveChangesAsync(cancellationToken);

        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToArray();

        return BuildResponse(user, roles);
    }

    private AuthResponse BuildResponse(User user, IReadOnlyList<string> roles)
    {
        var (token, expiresAt) = _tokenGenerator.GenerateToken(user, roles);

        return new AuthResponse
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles
            }
        };
    }

    private static string Normalize(string email) => email.Trim().ToLowerInvariant();
}
