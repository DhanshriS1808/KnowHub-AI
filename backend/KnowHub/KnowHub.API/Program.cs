using KnowHub.API.ExceptionHandling;
using KnowHub.API.Extensions;
using KnowHub.Application.Extensions;
using KnowHub.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiWithJwt();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "KnowHub API v1"));
}

// Authentication must run before authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "KnowHub API is running.");

// Reports Healthy only when a connection to PostgreSQL can be opened.
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
