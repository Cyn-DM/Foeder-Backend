namespace FoederBusiness.Helpers;

public class AuthSettings
{
    public string PrivateKey { get; }

    public AuthSettings(string privateKey)
    {
        PrivateKey = privateKey;
    }
}