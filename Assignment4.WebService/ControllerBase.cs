using Assignment4;
using Microsoft.EntityFrameworkCore;
using WebServiceLayer;
using DataServiceLayer;
using Mapster;

namespace WebServiceLayer;

public class ControllerBase
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // -- Connection string --
        // -- Try enviroment variable first, then fallback to appsetting, then default --
        string cs = Environment.GetEnvironmentVariable("NORTHWIND_CS")
                    ?? builder.Configuration.GetConnectionString("Northwind")
                    ?? "host=localhost;db=northwind;uid=postgres;pwd=@ccess93";

        // -- Add DbContext --
        // Scoped lifetime = one instance per request
        builder.Services.AddDbContext<NorthwindContext>(options => options.UseNpgsql(cs), ServiceLifetime.Scoped);

        // Register the DataService with a scoped lifetime
        builder.Services.AddScoped<IDataService, DataService>();

        // Mapster for easy DTO mapping
        builder.Services.AddMapster();

        // -- Add standard Web API services --
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // -- Enable Swagger --
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // -- Map endpoints to controllers --
        app.MapControllers();

        app.Run();
    }
}