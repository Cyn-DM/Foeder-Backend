using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IJwtAuthTokenUtils
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}