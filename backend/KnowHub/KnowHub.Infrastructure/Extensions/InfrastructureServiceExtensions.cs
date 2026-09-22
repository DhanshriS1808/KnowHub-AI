using KnowHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KnowHub.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    private const string ConnectionStringName = "KnowHubDb";

    /// <summary>
    /// Registers the PostgreSQL-backed persistence layer.
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

        return services;
    }
}
