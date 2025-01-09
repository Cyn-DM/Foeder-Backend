using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Google.Apis.Auth;
using Moq;

namespace FoederTest;

[TestFixture]
public class AuthServiceTests
{
    private (AuthService service, Mock<IGoogleTokenVerifier> mockGoogle, Mock<IAuthRepository> mockAuthRepo, Mock<IJwtAuthTokenUtils> mockUtils) GetSetup()
    {
        var mockGoogle = new Mock<IGoogleTokenVerifier>();
        var mockAuthRepo = new Mock<IAuthRepository>();
        var mockUtils = new Mock<IJwtAuthTokenUtils>();
        var service = new AuthService(mockGoogle.Object, mockAuthRepo.Object, mockUtils.Object);
        
        mockGoogle.Setup(m => m.VerifyIdToken(It.IsAny<string>()))
            .ReturnsAsync(() => new TokenVerificationResult{IsValid = true,
                payload = new GoogleJsonWebSignature.Payload{Email = "test@example.com", GivenName = "John", FamilyName = "Doe"}});
       
        mockAuthRepo.Setup(m => m.FindOrCreateUser(It.IsAny<User>()))
            .ReturnsAsync(new User { Email = "test@example.com", FirstName = "John", LastName = "Doe" });

        mockAuthRepo.Setup(m => m.StoreRefreshToken(It.IsAny<RefreshToken>()));
        
        mockAuthRepo.Setup(m => m.GetStoredRefreshToken(It.Is<string>(s => s == "valid_refresh_token")))
            .ReturnsAsync(new RefreshToken()
            {
                ExpirationDate = DateTime.Now.AddDays(2), User = new User(), Token = "test_refresh_token"
            });
        
        mockAuthRepo.Setup(m => m.GetStoredRefreshToken(It.Is<string>(s => s == "expired_refresh_token")))
            .ReturnsAsync(new RefreshToken()
            {
                ExpirationDate = DateTime.Now.AddDays(-2), User = new User(), Token = "expired_refresh_token"
            });
       
        mockUtils.Setup(m => m.GenerateAccessToken(It.IsAny<User>()))
            .Returns("test_access_token");
       
        mockUtils.Setup(m => m.GenerateRefreshToken())
            .Returns("test_refresh_token");

        return (service, mockGoogle, mockAuthRepo, mockUtils);
    }

    [Test]
    public async Task AssertLoginSuccessful()
    {
        //Arrange
       var setup = GetSetup();

       //Act
       var result = await setup.service.Login("valid_id_token");
       
       //Assert
       Assert.IsNotNull(result);
       Assert.AreEqual("test_access_token", result.AccessToken);
       Assert.AreEqual("test_refresh_token", result.RefreshToken);
       setup.mockAuthRepo.Verify(m => m.StoreRefreshToken(It.IsAny<RefreshToken>()), Times.Once);
       
    }

    [Test]
    public async Task AssertLoginFailed()
    {
        //Arrange
        var setup = GetSetup();
        setup.mockGoogle.Setup(m => m.VerifyIdToken(It.IsAny<string>()))
            .ReturnsAsync(() => new TokenVerificationResult { IsValid = false });
        
        //Act
        var result = await setup.service.Login("invalid_id_token");
        
        Assert.IsNull(result);
        setup.mockAuthRepo.Verify(m => m.StoreRefreshToken(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Test]
    public async Task AssertRefreshSuccessful()
    {
        //Arrange
        var setup = GetSetup();

        //Act
        var result = await setup.service.Refresh("valid_refresh_token");
        
        //Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.isRefreshTokenFound);
        Assert.IsFalse(result.IsRefreshTokenExpired);
        Assert.IsNotNull(result.AccessToken);
    }

    [Test]
    public async Task AssertRefreshFailedRefreshTokenNotFound()
    {
        //Arrange
        var setup = GetSetup();
        
        //Act
        var result = await setup.service.Refresh("invalid_refresh_token");
        
        //Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.isRefreshTokenFound);
    }

    [Test]
    public async Task AssertRefreshFailedRefreshTokenExpired()
    {
        //Arrange
        var setup = GetSetup();

        
        //Act
        var result = await setup.service.Refresh("expired_refresh_token");
        
        //Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.isRefreshTokenFound);
        Assert.IsTrue(result.IsRefreshTokenExpired);
        
        
    }
}