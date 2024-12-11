using FoederBusiness.Helpers;
using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IHouseholdService
{
    Task<ValidationDTO> AddHousehold(Household household, string bearerToken);
    Task<Household?> GetHouseholdByUserId(Guid userId);
    Task LeaveHousehold(Guid userId);
}