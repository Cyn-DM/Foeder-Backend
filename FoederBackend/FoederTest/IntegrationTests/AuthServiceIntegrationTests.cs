using System.Security.Cryptography;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL;
using FoederDAL.Repository;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Google.Apis.Auth;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FoederTest.IntegrationTests;

[TestFixture]
public class AuthServiceIntegrationTests
{
    private SqliteConnection _connection;
    private DbContextOptions<MssqlDbContext> _contextOptions;
    private AuthService _authService;


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
        _authService = new AuthService(mockGoogle.Object, mockAuthRepo, utils);

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
    public async Task AssertRefreshTokenSuccessful()
    {
        using var context = CreateContext();
        var refreshToken = "valid_token";

        var result = await _authService.Refresh(refreshToken);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.isRefreshTokenFound);
        Assert.IsFalse(result.IsRefreshTokenExpired);
        Assert.IsNotNull(result.AccessToken);

        Dispose();
    }

    [Test]
    public async Task AssertRefreshTokenNotFound()
    {
        using var context = CreateContext();
        var refreshToken = "invalid_token";

        var result = await _authService.Refresh(refreshToken);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.isRefreshTokenFound);
        Assert.IsFalse(result.IsRefreshTokenExpired);
        Assert.IsNull(result.AccessToken);

        Dispose();
    }

    [Test]
    public async Task AssertRefreshTokenExpired()
    {
        using var context = CreateContext();
        var refreshToken = "expired_token";

        var result = await _authService.Refresh(refreshToken);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.isRefreshTokenFound);
        Assert.IsTrue(result.IsRefreshTokenExpired);
        Assert.IsNull(result.AccessToken);

        Dispose();
    }

    [Test]
    public async Task AssertLoginSuccessful()
    {
        using var context = CreateContext();
        var idToken = "valid_id_token";

        var result = await _authService.Login(idToken);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.AccessToken);
        Assert.IsNotEmpty(result.AccessToken);
        Assert.IsNotNull(result.RefreshToken);
        Assert.IsNotEmpty(result.RefreshToken);
    }

    [Test]
    public async Task AssertLoginFailed()
    {
        using var context = CreateContext();
        var idToken = "invalid_id_token";

        var result = await _authService.Login(idToken);

        Assert.IsNull(result);
    }

}