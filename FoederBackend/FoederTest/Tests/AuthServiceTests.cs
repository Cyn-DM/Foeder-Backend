using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDomain.Interfaces;
using Google.Apis.Auth;
using Moq;

namespace FoederTest;

[TestFixture]
public class AuthServiceTests
{
    
    public (AuthService service, Mock<IGoogleTokenVerifier> googleMock, Mock<IAuthRepository> authRepoMock, Mock<JwtAuthTokenUtils> mockUtils) GetSetup()
    {
        var mockGoogle = new Mock<IGoogleTokenVerifier>();
        var mockAuthRepo = new Mock<IAuthRepository>();
        var mockUtils = new Mock<JwtAuthTokenUtils>();
        var service = new AuthService(mockGoogle.Object, mockAuthRepo.Object, mockUtils.Object);

        return (service, mockGoogle, mockAuthRepo, mockUtils);
    }

    public void AssertLoginSuccesful()
    {
       var setup = GetSetup();
       setup.googleMock.Setup(m => m.VerifyIdToken(It.IsAny<string>()))
           .Returns(() => new TokenVerificationResult{IsValid = true,
               payload = new GoogleJsonWebSignature.Payload{}});
       setup.mockUtils.Setup(m => m.GenerateAccessToken())



    }
}