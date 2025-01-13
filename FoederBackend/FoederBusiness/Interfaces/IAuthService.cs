using FoederBusiness.Dtos;

namespace FoederBusiness.Interfaces;

public interface IAuthService
{
    Task<LoginTokenResult?> Login(string authToken);
    Task<RefreshResult?> Refresh(string refreshToken);
}