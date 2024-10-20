using FoederDomain.DomainModels;
using Google.Apis.Auth;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using ILogger = Google.Apis.Logging.ILogger;

namespace FoederBusiness.Tools;

public class TokenVerificationResult
{
    public bool isValid { get; set; }
    public string errorMessage { get; set; }
    public GoogleJsonWebSignature.Payload payload { get; set; }

}

public class TokenVerifier
{
    private readonly ILogger<TokenVerifier> _logger;

    public TokenVerifier(ILogger<TokenVerifier> logger)
    {
        _logger = logger;
    }

    public async Task<TokenVerificationResult> VerifyIdToken(string idToken)
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return result;
    }
}