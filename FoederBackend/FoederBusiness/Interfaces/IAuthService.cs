using FoederBusiness.Dtos;

namespace FoederBusiness.Interfaces;

public interface IAuthService
{
    Task<TokenResult?> Login(string authToken);
}