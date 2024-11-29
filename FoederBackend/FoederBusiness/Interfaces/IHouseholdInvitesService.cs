using FoederBusiness.Helpers;
using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IHouseholdInvitesService
{
    Task<List<HouseholdInvite>> GetHouseholdInvites(Guid userId);
    Task<ValidationDTO> InviteToHousehold(HouseholdInvite householdInvite);
    Task RespondToHouseholdInvite(HouseholdInvite householdInvite);
}