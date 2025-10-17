using Assignment4;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace WebServiceLayer;

/// <summary>
/// Entry point for the ASP.NET Core Web API application.
/// Configures dependency injection, database context, and HTTP request pipeline.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        // Create the web application builder - provides configuration and service registration
        var builder = WebApplication.CreateBuilder(args);

        // -- Connection string resolution strategy --
        // Priority order:
        // 1. Environment variable (for production/CI/CD flexibility)
        // 2. appsettings.json configuration (for local development)
        // 3. Hardcoded default (fallback for quick local testing)
        string cs = Environment.GetEnvironmentVariable("NORTHWIND_CS")
                    ?? builder.Configuration.GetConnectionString("Northwind")
                    ?? "host=localhost;db=northwind;uid=postgres;pwd=@ccess93";

        // -- Register DbContext with Dependency Injection --
        // ServiceLifetime.Scoped = one instance per HTTP request
        // This ensures proper connection management and prevents threading issues
        // Each HTTP request gets its own DbContext instance that's disposed after the request
        builder.Services.AddDbContext<NorthwindContext>(options => options.UseNpgsql(cs), ServiceLifetime.Scoped);

        // -- Register DataService implementation --
        // Controllers receive IDataService, but get DataService implementation injected
        // Scoped lifetime aligns with DbContext - same instance used throughout one request
        builder.Services.AddScoped<IDataService, DataService>();

        // -- Register Mapster for DTO mapping --
        // Mapster simplifies conversion between domain models and DTOs
        // (Though in this project, we're using manual anonymous objects and DTOs)
        builder.Services.AddMapster();

        // -- Add standard Web API services --
        // AddControllers: Enables MVC controller support for handling HTTP requests
        // AddEndpointsApiExplorer: Enables endpoint discovery for API documentation
        // AddSwaggerGen: Generates OpenAPI/Swagger specification for API documentation
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Build the application with all configured services
        var app = builder.Build();

        // -- Configure HTTP request pipeline --
        
        // Enable Swagger UI only in development environment
        // Swagger provides interactive API documentation at /swagger
        // Don't expose in production for security reasons
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();      // Serves OpenAPI JSON at /swagger/v1/swagger.json
            app.UseSwaggerUI();    // Serves interactive UI at /swagger
        }

        // Map HTTP requests to controller actions based on routing attributes
        // Controllers use [Route] and [HttpGet]/[HttpPost]/etc. attributes for routing
        app.MapControllers();

        // Start the web server and begin listening for HTTP requests
        // Blocks until the application is shut down
        app.Run();
    }
}