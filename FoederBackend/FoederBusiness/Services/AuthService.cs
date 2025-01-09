using FoederBusiness.Dtos;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;

namespace FoederBusiness.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGoogleTokenVerifier _googleTokenVerifier;
        private readonly IAuthRepository _authRepo;
        private readonly IJwtAuthTokenUtils _jwtAuthTokenUtils;

        public AuthService(IGoogleTokenVerifier googleTokenVerifier, IAuthRepository authRepo, IJwtAuthTokenUtils jwtAuthTokenUtils)
        {
            _googleTokenVerifier = googleTokenVerifier;
            _authRepo = authRepo;
            _jwtAuthTokenUtils = jwtAuthTokenUtils;
        }

        public async Task<LoginTokenResult?> Login(string authToken)
        {
            var tokenResult = await _googleTokenVerifier.VerifyIdToken(authToken);
            

            if (tokenResult.IsValid == false)
            {
                return null;
            }

            User verifiedGoogleUser = new User()
            {
                Email = tokenResult.payload!.Email,
                FirstName = tokenResult.payload.GivenName,
                LastName = tokenResult.payload.FamilyName,
            };

            User foederUser = await _authRepo.FindOrCreateUser(verifiedGoogleUser); 
            string accessToken = _jwtAuthTokenUtils.GenerateAccessToken(foederUser);
            string refreshToken = _jwtAuthTokenUtils.GenerateRefreshToken();
            
            
            
            
            
            _authRepo.StoreRefreshToken(new RefreshToken(){ Token = refreshToken, ExpirationDate = DateTime.Now.AddDays(1), User = foederUser });
            
            return new LoginTokenResult() { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<RefreshResult?> Refresh(string refreshToken)
        {
            var storedRefreshToken = await _authRepo.GetStoredRefreshToken(refreshToken);
            var refreshResult = new RefreshResult();
            
            if (storedRefreshToken == null)
            {
                refreshResult.isRefreshTokenFound = false;
                return refreshResult;
            }
            
            if (storedRefreshToken.ExpirationDate < DateTime.Now)
            {
                refreshResult.IsRefreshTokenExpired = true;
                refreshResult.isRefreshTokenFound = true;
                return refreshResult;
            }
            
            refreshResult.AccessToken = _jwtAuthTokenUtils.GenerateAccessToken(storedRefreshToken.User);
            refreshResult.isRefreshTokenFound = true;
            refreshResult.IsRefreshTokenExpired = false;
            
            return refreshResult;
        }
        

    }
}
