using AccountService.Infrastructure.Persistence;
using AccountService.Application.Interfaces;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---

// PostgreSQL connection string from environment or appsettings
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
