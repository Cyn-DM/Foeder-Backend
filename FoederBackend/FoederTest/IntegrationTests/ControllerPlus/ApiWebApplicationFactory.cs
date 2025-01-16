using System.Security.Cryptography;
using FoederAPI.Controllers;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL;
using FoederDAL.Repository;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace FoederTest.IntegrationTests.ControllerPlus;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
{
    var connection = new SqliteConnection("Filename=:memory:");
    connection.Open();

    builder.ConfigureTestServices(services =>
    {
        // Register the SQLite connection so it can be reused
        services.AddSingleton(connection);

        // Configure the DbContext to use the shared SQLite connection
        services.AddDbContext<MssqlDbContext>(options => options.UseSqlite(connection));


        byte[] keyBytes = new byte[48];
        RandomNumberGenerator.Fill(keyBytes);
        string base64Key = Convert.ToBase64String(keyBytes);
        
        var mockGoogle = new Mock<IGoogleTokenVerifier>();
        var mockAuthRepo = new AuthRepository(new MssqlDbContext(
            new DbContextOptionsBuilder<MssqlDbContext>().UseSqlite(connection).Options));
        var utils = new JwtAuthTokenUtils(new AuthSettings(base64Key, "test", "test", "30"));
        var authService = new AuthService(mockGoogle.Object, mockAuthRepo, utils);

        var validIdToken = "valid_id_token";
        var invalidIdToken = "invalid_id_token";

        mockGoogle.Setup(m => m.VerifyIdToken(It.Is<string>(to => to == validIdToken)))
            .ReturnsAsync(new TokenVerificationResult
            {
                IsValid = true,
                payload = new GoogleJsonWebSignature.Payload
                {
                    Email = "test@example.com",
                    GivenName = "John",
                    FamilyName = "Doe"
                }
            });

        mockGoogle.Setup(m => m.VerifyIdToken(It.Is<string>(to => to == invalidIdToken)))
            .ReturnsAsync(new TokenVerificationResult { IsValid = false });
        
        services.AddScoped<IAuthService>(provider => authService);
        services.AddScoped<IAuthRepository>(provider => mockAuthRepo);
        services.AddScoped<JwtAuthTokenUtils>(provider => utils);
        services.AddScoped<IGoogleTokenVerifier>(provider => mockGoogle.Object);
        
        var context = services.BuildServiceProvider().GetRequiredService<MssqlDbContext>();
        SeedDatabase(context);
    });
}
    private static void SeedDatabase(MssqlDbContext context)
    {
        if (context.Database.EnsureCreated())
        {
            var john = new User { Email = "test@example.com", FirstName = "John", LastName = "Doe" };
            context.AddRange(
                john,
                new RefreshToken
                {
                    Token = "valid_token",
                    ExpirationDate = DateTime.Now.AddDays(3),
                    User = john
                },
                new RefreshToken
                {
                    Token = "expired_token",
                    ExpirationDate = DateTime.Now.AddDays(-3),
                    User = john
                });
            context.SaveChanges();
        }
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var connection = Services.GetService<SqliteConnection>();
            connection?.Dispose();
        }

        base.Dispose(disposing);
    }

    public override async ValueTask DisposeAsync()
    {
        var connection = Services.GetService<SqliteConnection>();
        if (connection != null)
        {
            await connection.DisposeAsync();
        }

        await base.DisposeAsync();
    }
}