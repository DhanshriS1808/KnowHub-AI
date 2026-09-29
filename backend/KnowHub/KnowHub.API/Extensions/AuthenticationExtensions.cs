using System.Text;
using KnowHub.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace KnowHub.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var settings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException($"Missing '{JwtSettings.SectionName}' configuration section.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Without this the handler rewrites "sub" and "email" into long
                // WS-Federation URIs, and lookups by the original name fail.
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(settings.Key)),
                    ValidateLifetime = true,
                    // Default is 5 minutes of leeway, which lets expired tokens work.
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = JwtClaimNames.Name,
                    RoleClaimType = JwtClaimNames.Role
                };

                if (!environment.IsDevelopment())
                {
                    return;
                }

                // Development only: a 401 says nothing by itself, so log why the
                // token was rejected and echo the raw header that arrived.
                options.IncludeErrorDetails = true;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JwtDiagnostics");

                        logger.LogWarning(
                            context.Exception,
                            "JWT rejected on {Path}",
                            context.HttpContext.Request.Path);

                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var header = context.HttpContext.Request.Headers.Authorization.ToString();

                        if (!string.IsNullOrEmpty(header))
                        {
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILoggerFactory>()
                                .CreateLogger("JwtDiagnostics");

                            // Length and prefix only. The token itself is a credential
                            // and is never written to the log.
                            logger.LogInformation(
                                "Authorization header received: starts with '{Prefix}', total length {Length}",
                                header.Length >= 16 ? header[..16] : header,
                                header.Length);
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}
