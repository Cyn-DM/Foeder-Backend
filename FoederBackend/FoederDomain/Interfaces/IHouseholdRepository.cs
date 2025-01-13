using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IHouseholdRepository
{
    Task AddHousehold(Household household, User user);
    Task<Household?> GetHouseholdByUserId(Guid userId);
    Task<Household?> GetHouseholdById(Guid householdId);
    Task LeaveHousehold(Household household, User user);
}