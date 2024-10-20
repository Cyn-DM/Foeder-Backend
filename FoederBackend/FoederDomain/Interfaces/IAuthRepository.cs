using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IAuthRepository
{
    User FindOrCreateUser(User user);
}