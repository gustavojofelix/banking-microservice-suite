using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---

// PostgreSQL connection string from environment or appsettings
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add Swagger/OpenAPI
builder.Services.AddOpenApi(); // or switch to AddEndpointsApiExplorer + AddSwaggerGen

var app = builder.Build();

// Map Swagger only in Development
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

// Health check endpoint
app.MapGet("/health", () => Results.Ok("AccountService is running!"));

// Future: Register more endpoints and DI here

app.Run();
