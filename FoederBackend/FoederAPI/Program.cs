using FoederDAL;
using Microsoft.EntityFrameworkCore;
using System;
using FoederBusiness;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL.Repository;
using FoederDomain.Interfaces;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting API.");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Configuration.AddUserSecrets<Program>();
    var dbConnectionString = builder.Configuration["DbConnectionString"];
    var jwtSecret = builder.Configuration["JwtSecret"];

    var dbContext = new MssqlDbContext(builder.Configuration);
    builder.Services.AddSingleton<DbContext>(sp => dbContext);
    builder.Services.AddSingleton<TokenVerifier>();
    builder.Services.AddSingleton<IRecipeRepository, RecipeRepository>();
    builder.Services.AddSingleton<IRecipeService, RecipeService>();
    builder.Services.AddSingleton<IAuthService, AuthService>();
    builder.Services.AddSingleton<IAuthRepository, AuthRepository>();
    builder.Services.AddSingleton<AuthSettings>(sp => new AuthSettings(jwtSecret));


    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins("https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapGet("/", () => dbConnectionString);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

    }

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DbContext>();


            // Seed the data
            var seeder = new DataSeeder(context as MssqlDbContext);
            seeder.Seed();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }


    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}


