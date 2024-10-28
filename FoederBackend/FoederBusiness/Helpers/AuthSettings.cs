namespace FoederBusiness.Helpers;

public class AuthSettings
{
    public string PrivateKey { get; }
    public string Issuer { get; }
    public string Audience { get; }
    public string Expiration { get; }

    public AuthSettings(string privateKey, string issuer, string audience, string expiration)
    {
        PrivateKey = privateKey;
        Issuer = issuer;
        Audience = audience;
        Expiration = expiration;
    }
}