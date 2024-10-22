using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;

namespace FoederBusiness.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenVerifier _tokenVerifier;
        private readonly IAuthRepository _authRepo;
        private readonly AuthSettings _authSettings;

        public AuthService(TokenVerifier tokenVerifier, IAuthRepository authRepo, AuthSettings authSettings)
        {
            _tokenVerifier = tokenVerifier;
            _authRepo = authRepo;
            _authSettings = authSettings;
        }

        public async Task<TokenVerificationResult> VerifyGoogleIdToken(string idToken)
        {
            return await _tokenVerifier.VerifyIdToken(idToken);
            
        }

        public async Task<string?> Login(string idToken)
        {
            var tokenResult = await _tokenVerifier.VerifyIdToken(idToken);

            if (!tokenResult.isValid)
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

            string token = GenerateToken(foederUser);

            return token;

        }
        private User FindOrCreateUser(User user)
        {
            return _authRepo.FindOrCreateUser(user);
        }

        public string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.PrivateKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.FullName));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            if (user.Household != null)
            {
                claims.AddClaim(new Claim("HouseholdId", user.Household.Id.ToString()));
            }

            return claims;

        }

    }


}
