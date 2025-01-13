using Google.Apis.Auth;
using Microsoft.Extensions.Logging;

namespace FoederBusiness.Tools;

public class TokenVerificationResult
{
    public bool? IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public GoogleJsonWebSignature.Payload? payload { get; set; }

}

public class GoogleTokenVerifier : IGoogleTokenVerifier
{   
    private readonly ILogger<GoogleTokenVerifier> _logger;

    public GoogleTokenVerifier(ILogger<GoogleTokenVerifier> logger)
    {
        _logger = logger;
    }

    public async Task<TokenVerificationResult> VerifyIdToken(string idToken)
    {
        var result = new TokenVerificationResult();

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            result.IsValid = true;
            result.payload = payload;
        }
        catch (InvalidJwtException ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return result;
    }
}