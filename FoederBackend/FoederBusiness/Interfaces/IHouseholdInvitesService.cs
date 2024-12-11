using FoederBusiness.Helpers;
using FoederDomain.DomainModels;

namespace FoederBusiness.Interfaces;

public interface IHouseholdInvitesService
{
    Task<List<HouseholdInvite>> GetHouseholdInvites(Guid userId);
    Task<ValidationDTO> InviteToHousehold(string email, Guid householdId);
    Task RespondToHouseholdInvite(Guid inviteId, bool isAccepted);
}