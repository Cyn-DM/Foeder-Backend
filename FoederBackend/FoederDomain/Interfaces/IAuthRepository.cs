using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IAuthRepository
{
    Task<User> FindOrCreateUser(User user);
    Task<RefreshToken?> GetStoredRefreshToken(string refreshToken);
    void StoreRefreshToken(RefreshToken refreshToken);
}