using FoederBusiness.Tools;

namespace FoederBusiness.Interfaces;

public interface IAuthService
{
    Task<TokenVerificationResult> VerifyGoogleIdToken(string authToken);
}