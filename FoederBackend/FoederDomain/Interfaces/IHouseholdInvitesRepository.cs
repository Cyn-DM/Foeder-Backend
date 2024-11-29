using FoederDomain.DomainModels;

namespace FoederDomain.Interfaces;

public interface IHouseholdInvitesRepository
{
    Task<List<HouseholdInvite>> GetHouseholdInvites(Guid userId);
    Task InviteToHousehold(HouseholdInvite householdInvite);
    Task RespondToHouseholdInvite(Guid householdInviteId, bool isAccepted);
}