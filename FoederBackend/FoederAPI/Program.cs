using FoederDAL;
using Microsoft.EntityFrameworkCore;
using System;
using FoederBusiness;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederDAL.Repository;
using FoederDomain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Localhost",
        policy =>
        {
            policy.WithOrigins("https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Logging.AddConsole();
builder.Services.AddSwaggerGen();
builder.Configuration.AddUserSecrets<Program>();
var dbConnectionString = builder.Configuration["DbConnectionString"];

var dbcontext = new MssqlDbContext(builder.Configuration);
builder.Services.AddSingleton<DbContext>(sp => dbcontext);
var recipeRepo = new RecipeRepository(dbcontext);
builder.Services.AddSingleton<IRecipeRepository>(sp => recipeRepo );
builder.Services.AddSingleton<IRecipeService>(sp => new RecipeService(recipeRepo));
builder.Services.AddSingleton<IAuthService>(sp => new AuthService());

var app = builder.Build();

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

app.UseCors("Localhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();