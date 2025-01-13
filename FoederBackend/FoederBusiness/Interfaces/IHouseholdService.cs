using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IHouseholdService
{
    Task AddHousehold(Household household, string bearerToken);
    Task<Household?> GetHouseholdByUserId(Guid userId);
    Task LeaveHousehold(Guid userId);
}