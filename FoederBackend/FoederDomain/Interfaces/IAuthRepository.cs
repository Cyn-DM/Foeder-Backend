using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IAuthRepository
{
    Task<User> FindOrCreateUser(User user);
    Task<User?> FindUserByEmail(string email);
    Task<User?> FindUserById(Guid userId);
    Task<RefreshToken?> GetStoredRefreshToken(string refreshToken);
    void StoreRefreshToken(RefreshToken refreshToken);
}