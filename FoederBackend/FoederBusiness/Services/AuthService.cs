using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FoederBusiness.Dtos;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace FoederBusiness.Services
{
    public class AuthService : IAuthService
    {
        private readonly GoogleTokenVerifier _googleTokenVerifier;
        private readonly IAuthRepository _authRepo;
        private readonly JwtAuthTokenUtils _jwtAuthTokenUtils;

        public AuthService(GoogleTokenVerifier googleTokenVerifier, IAuthRepository authRepo, JwtAuthTokenUtils jwtAuthTokenUtils)
        {
            _googleTokenVerifier = googleTokenVerifier;
            _authRepo = authRepo;
            _jwtAuthTokenUtils = jwtAuthTokenUtils;
        }

        public async Task<TokenResult?> Login(string idToken)
        {
            var tokenResult = await _googleTokenVerifier.VerifyIdToken(idToken);

            if (tokenResult.IsValid == false)
            {
                return null;
            }

            if (tokenResult.payload == null)
            {
                return null;
            }

            User verifiedGoogleUser = new User()
            {
                Email = tokenResult.payload.Email,
                FirstName = tokenResult.payload.GivenName,
                LastName = tokenResult.payload.FamilyName,
            };

            User foederUser = FindOrCreateUser(verifiedGoogleUser);
            string accessToken = _jwtAuthTokenUtils.GenerateAccessToken(foederUser);
            string refreshToken = _jwtAuthTokenUtils.GenerateRefreshToken();


            return new TokenResult() { AccessToken = accessToken, RefreshToken = refreshToken };

        }
        private User FindOrCreateUser(User user)
        {
            return _authRepo.FindOrCreateUser(user);
        }
    }
}
