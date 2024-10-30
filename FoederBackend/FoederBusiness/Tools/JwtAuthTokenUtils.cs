using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederDomain.DomainModels;
using Microsoft.IdentityModel.Tokens;

namespace FoederBusiness.Services;

public class JwtAuthTokenUtils : IJwtAuthTokenUtils
{
    private readonly AuthSettings _authSettings;


    public JwtAuthTokenUtils(AuthSettings authSettings)
    {
        _authSettings = authSettings;
    }

    public string GenerateAccessToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSettings.PrivateKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.Now.AddMinutes(Int32.Parse(_authSettings.Expiration)),
            SigningCredentials = credentials,
            Issuer = _authSettings.Issuer,
            Audience = _authSettings.Audience,
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
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}