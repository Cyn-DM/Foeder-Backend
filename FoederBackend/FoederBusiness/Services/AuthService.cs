using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using Google.Apis.Auth;

namespace FoederBusiness.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenVerifier _tokenVerifier;

        public AuthService(TokenVerifier tokenVerifier)
        {
            _tokenVerifier = tokenVerifier;
        }

        public async Task<TokenVerificationResult> VerifyGoogleIdToken(string idToken)
        {
            try
            {
                return await _tokenVerifier.VerifyIdToken(idToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }

    }
}
