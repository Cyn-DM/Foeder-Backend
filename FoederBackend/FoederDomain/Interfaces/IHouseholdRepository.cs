using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IHouseholdRepository
{
    public Task AddHousehold(Household household, User user);
}