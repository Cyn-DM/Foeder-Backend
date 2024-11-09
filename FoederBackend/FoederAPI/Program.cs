using FoederDAL;
using Microsoft.EntityFrameworkCore;
using System.Text;
using FoederBusiness;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL.Repository;
using FoederDomain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
    builder.Configuration.AddUserSecrets<Program>(optional : true)
        .AddEnvironmentVariables();
    var dbConnectionString = builder.Configuration["DbConnectionString"];
    var jwtSecret = builder.Configuration["JwtSettings:SecretKey"];
    var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
    var jwtAudience = builder.Configuration["JwtSettings:Audience"];
    var jwtExpiration = builder.Configuration["JwtSettings:AccessTokenExpiration"];
    builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret!)),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
            };

        });
    builder.Services.AddAuthorization();
    
    //Dependencies
    builder.Services.AddDbContext<MssqlDbContext>(options => options.UseSqlServer(dbConnectionString));
    builder.Services.AddScoped<IJwtAuthTokenUtils, JwtAuthTokenUtils>();
    builder.Services.AddScoped<IGoogleTokenVerifier, GoogleTokenVerifier>();
    builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
    builder.Services.AddScoped<IRecipeService, RecipeService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddSingleton<AuthSettings>(sp => new AuthSettings(jwtSecret ?? throw new Exception("Add jwtSecret."),
        jwtIssuer ?? throw new Exception("Add issuer."),
        jwtAudience ?? throw new Exception("Add audience."),
        jwtExpiration ?? throw new Exception("Add expiration.")));


    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins("https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapGet("/", () => Results.Redirect("/swagger"));

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
            var context = services.GetRequiredService<MssqlDbContext>();


            // Seed the data
            var seeder = new DataSeeder(context);
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

    app.UseAuthentication();

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


