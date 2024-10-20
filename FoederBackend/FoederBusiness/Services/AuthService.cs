using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Google.Apis.Auth;

namespace FoederBusiness.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenVerifier _tokenVerifier;
        private readonly IAuthRepository _authRepo;

        public AuthService(TokenVerifier tokenVerifier, IAuthRepository authRepo)
        {
            _tokenVerifier = tokenVerifier;
            _authRepo = authRepo;
        }

        public async Task<TokenVerificationResult> VerifyGoogleIdToken(string idToken)
        {
            return await _tokenVerifier.VerifyIdToken(idToken);
            
        }

        public async Task<LoginResult> Login(string idToken)
        {
            var tokenResult = await _tokenVerifier.VerifyIdToken(idToken);

            if (!tokenResult.isValid)
            {
                return new LoginResult()
                {
                    TokenVerificationResult = tokenResult
                };
            }

            var loginResult = new LoginResult()
            {
                TokenVerificationResult = tokenResult,
                User = new User()
                {
                    Email = tokenResult.payload.Email,
                    FirstName = tokenResult.payload.GivenName,
                    LastName = tokenResult.payload.FamilyName,

                }
            };

            return loginResult;

        }
        private User FindOrCreateUser(User user)
        {
            return _authRepo.FindOrCreateUser(user);
        }
    }

   

    public class LoginResult
    {
        public TokenVerificationResult TokenVerificationResult { get; set; }
        public User? User { get; set; } = null;
    }
}
