namespace FoederBusiness.Interfaces;

public interface IAuthService
{
    bool VerifyGoogleIdToken(string authToken);
}