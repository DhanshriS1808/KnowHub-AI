using KnowHub.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "KnowHub API is running.");

// Reports Healthy only when a connection to PostgreSQL can be opened.
app.MapHealthChecks("/health");

app.Run();
