using FoederDomain.DomainModels;
using Google.Apis.Auth;

namespace FoederBusiness.Tools;

public class TokenVerificationResult
{
    public bool isValid { get; set; }
    public string errorMessage { get; set; }
    public GoogleJsonWebSignature.Payload payload { get; set; }

}

public class TokenVerifier
{
    public static async Task<TokenVerificationResult> VerifyIdToken(string idToken)
    {
        var result = new TokenVerificationResult();

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            result.isValid = true;
            result.payload = payload;
        }
        catch (InvalidJwtException ex)
        {
            result.isValid = false;
            result.errorMessage = ex.Message;
        }

        return result;
    }
}