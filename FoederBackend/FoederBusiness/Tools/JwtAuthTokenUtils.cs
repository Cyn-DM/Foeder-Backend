using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederDomain.DomainModels;
using Microsoft.AspNetCore.Http;
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

        claims.AddClaim(new Claim("Id", user.Id.ToString()));

        return claims;

    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            var baseString = Convert.ToBase64String(randomNumber);

            return HttpUtility.UrlEncode(baseString);
        }
    }

    public static string? GetUserEmailFromToken(string bearerToken)
    {
        var token = bearerToken.Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var readToken = handler.ReadJwtToken(token);
        
        return readToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        
    }

    public static Guid? GetUserHouseholdIdFromToken(string bearerToken)
    {
        var token = bearerToken.Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var readToken = handler.ReadJwtToken(token);
        
        var householdIdString = readToken.Claims.FirstOrDefault(c => c.Type == "HouseholdId")?.Value;

        try
        {
            return Guid.Parse(householdIdString);
        }
        catch (ArgumentNullException)
        {
            return null;
        }
        catch (FormatException)
        {
            throw new InvalidHouseholdIdException();
        }
        
    }
}

public class InvalidHouseholdIdException : Exception
{
    public override string Message => "Invalid Household Id. Please try again.";
}