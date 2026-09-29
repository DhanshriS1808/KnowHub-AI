using KnowHub.Application.Interfaces.Authentication;
using KnowHub.Application.Interfaces.Repositories;
using KnowHub.Infrastructure.Authentication;
using KnowHub.Infrastructure.Data;
using KnowHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KnowHub.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    private const string ConnectionStringName = "KnowHubDb";

    /// <summary>
    /// Registers the PostgreSQL-backed persistence layer and auth primitives.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' was not found. " +
                "Set it in appsettings.Development.json or via user secrets: " +
                $"dotnet user-secrets set \"ConnectionStrings:{ConnectionStringName}\" \"<value>\"");
        }

        services.AddDbContext<KnowHubDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(KnowHubDbContext).Assembly.FullName);
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory");
                npgsql.EnableRetryOnFailure();
            }));

        services.AddHealthChecks()
            .AddDbContextCheck<KnowHubDbContext>(name: "postgres");

        // Fails at startup rather than on the first login if the signing key is
        // missing or too short for HMAC-SHA256.
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .Validate(
                s => !string.IsNullOrWhiteSpace(s.Key) && s.Key.Length >= 32,
                "Jwt:Key is missing or shorter than 32 characters. Set it with: " +
                "dotnet user-secrets set \"Jwt:Key\" \"<at-least-32-chars>\"")
            .Validate(s => !string.IsNullOrWhiteSpace(s.Issuer), "Jwt:Issuer is required.")
            .Validate(s => !string.IsNullOrWhiteSpace(s.Audience), "Jwt:Audience is required.")
            .ValidateOnStart();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
