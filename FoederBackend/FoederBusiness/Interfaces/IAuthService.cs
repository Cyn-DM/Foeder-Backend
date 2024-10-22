using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IAuthService
{
    Task<User?> Login(string authToken);
}