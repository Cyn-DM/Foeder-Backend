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
    
    public (AuthService service, Mock<IGoogleTokenVerifier> mockGoogle, Mock<IAuthRepository> mockAuthRepo, Mock<IJwtAuthTokenUtils> mockUtils) GetSetup()
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
       
        mockUtils.Setup(m => m.GenerateAccessToken(It.IsAny<User>()))
            .Returns("test_access_token");
       
        mockUtils.Setup(m => m.GenerateRefreshToken())
            .Returns("test_refresh_token");

        return (service, mockGoogle, mockAuthRepo, mockUtils);
    }

    [Test]
    public async Task AssertLoginSuccesful()
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
}