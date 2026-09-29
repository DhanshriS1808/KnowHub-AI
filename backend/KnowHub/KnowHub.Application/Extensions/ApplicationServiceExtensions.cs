using KnowHub.Application.Interfaces.Services;
using KnowHub.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KnowHub.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
