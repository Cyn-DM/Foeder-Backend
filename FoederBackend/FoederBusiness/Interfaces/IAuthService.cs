using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IAuthService
{
    Task<string?> Login(string authToken);
}