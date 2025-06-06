var builder = WebApplication.CreateBuilder(args);

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
