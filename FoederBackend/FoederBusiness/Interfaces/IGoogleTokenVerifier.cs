namespace FoederBusiness.Tools;

public interface IGoogleTokenVerifier
{
    Task<TokenVerificationResult> VerifyIdToken(string idToken);
}