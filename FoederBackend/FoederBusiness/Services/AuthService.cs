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
        public AuthService() { }

        public async Task<TokenVerificationResult> VerifyGoogleIdToken(string idToken)
        {
            return await TokenVerifier.VerifyIdToken(idToken);
        }


    }
}
