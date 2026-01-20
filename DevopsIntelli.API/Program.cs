using DevopsIntelli.API.Middleware;
using DevopsIntelli.Infrastructure;
using DevopsIntelli.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<DevopsIntelliDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("defaultConnection");
    options.UseNpgsql((connectionString), b =>
    {
        b.MigrationsAssembly("DevopsIntelli.Infrastructure");
        b.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    });

});
builder.Services.AddHa(config =>config.UsePostgresqlStorage)
//DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration).AddControllers;
builder.Services.AddInfrastructure(builder.Configuration);
//add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//add problemdetails for response
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DevopsIntelliDBContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Database migrated successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Database migration failed: {ex.Message}");
        Console.WriteLine("App will start anyway - check database connection");
        // Don't crash - let Swagger still work
    }
}

app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//use exception extension
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
