using System.Security.Cryptography;
using FoederAPI.Controllers;
using FoederBusiness.Helpers;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL;
using FoederDAL.Repository;
using FoederDomain.DomainModels;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FoederTest.IntegrationTests.ControllerPlus;

[TestFixture]
public class AuthControllerIntegrationTest
{
    private SqliteConnection _connection;
    private DbContextOptions<MssqlDbContext> _contextOptions;
    private AuthController _authController;


    [SetUp]
    public void SetUp()
    {
        // Set up in memory database
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<MssqlDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new MssqlDbContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            var john = new User { Email = "test@example.com", FirstName = "John", LastName = "Doe" };
            context.AddRange(
                john,
                new RefreshToken()
                {
                    Token = "valid_token",
                    ExpirationDate = DateTime.Now.AddDays(3),
                    User = john,
                },
                new RefreshToken()
                {
                    Token = "expired_token",
                    ExpirationDate = DateTime.Now.AddDays(-3),
                    User = john,
                });
            context.SaveChanges();
        }

        byte[] keyBytes = new byte[48];
        RandomNumberGenerator.Fill(keyBytes);
        string base64Key = Convert.ToBase64String(keyBytes);

        var mockGoogle = new Mock<IGoogleTokenVerifier>();
        var mockAuthRepo = new AuthRepository(context);
        var utils = new JwtAuthTokenUtils(new AuthSettings(base64Key, "test", "test", "30"));
        var authService = new AuthService(mockGoogle.Object, mockAuthRepo, utils);
        _authController = new AuthController(authService);
        var httpContext = new DefaultHttpContext();
        _authController.ControllerContext = new ControllerContext() { HttpContext = httpContext };

        var validIdToken = "valid_id_token";
        var invalidIdToken = "invalid_id_token";

        mockGoogle.Setup(m => m.VerifyIdToken(It.Is<string>((to) => to == validIdToken)))
            .ReturnsAsync(() => new TokenVerificationResult
            {
                IsValid = true,
                payload = new GoogleJsonWebSignature.Payload
                    { Email = "test@example.com", GivenName = "John", FamilyName = "Doe" }
            });

        mockGoogle.Setup(m => m.VerifyIdToken(It.Is<string>((to) => to == invalidIdToken)))
            .ReturnsAsync(() => new TokenVerificationResult
            {
                IsValid = false,
            });
    }

    private MssqlDbContext CreateContext() => new MssqlDbContext(_contextOptions);

    [TearDown]
    public void Dispose() => _connection.Dispose();


    [Test]
    public async Task LoginSuccessfull()
    {
        using var context = CreateContext();
        var request = new Response()
        {
            CredentialResponse = "valid_id_token",
        };
        
        var result = await _authController.LogIn(request);
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Dispose();
    }
    
    [Test]
    public async Task AssertLoginFailed()
    {
        using var context = CreateContext();
        var idToken = "invalid_id_token";

        var result = await _authController.LogIn(new Response(){CredentialResponse = idToken});
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
        Dispose();
    }
    
}