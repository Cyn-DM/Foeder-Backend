using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;

namespace FoederBusiness.Services;

public class HouseholdInvitesService : IHouseholdInvitesService
{
    private readonly IHouseholdInvitesRepository _householdInvitesRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IHouseholdRepository _householdRepository;
    private readonly IInviteNotifier _notifier;

    public HouseholdInvitesService(IHouseholdInvitesRepository householdInvitesRepository, IAuthRepository authRepository, IHouseholdRepository householdRepository, IInviteNotifier notifier)
    {
        _householdInvitesRepository = householdInvitesRepository;
        _authRepository = authRepository;
        _householdRepository = householdRepository;
        _notifier = notifier;
    }
    public async Task<List<HouseholdInvite>> GetHouseholdInvites(Guid userId)
    {
        if (await _authRepository.FindUserById(userId) == null)
        {
            throw new UserNotFoundException();
        }
        return await _householdInvitesRepository.GetHouseholdInvites(userId);
    }

    public async Task InviteToHousehold(string email, Guid householdId)
    {
        
            var user = await _authRepository.FindUserByEmail(email);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            if (user.Household != null)
            {
                throw new UserAlreadyHasHouseholdException();
            }

            var household = await _householdRepository.GetHouseholdById(householdId);

            if (household is null)
            {
                throw new HouseholdNotFoundException();
            }

            var householdInvite = new HouseholdInvite();
            householdInvite.Household = household;
            householdInvite.InvitedUser = user;

            await _householdInvitesRepository.InviteToHousehold(householdInvite);

            var message = $"{householdInvite.Household.Name} has invited you to the household.";
            
            await _notifier.NotifyInvite(user.Id, message);
    }

    public async Task RespondToHouseholdInvite(Guid inviteId, bool isAccepted)
    {
        var invite = await _householdInvitesRepository.GetHouseholdInviteById(inviteId);
        
        invite.IsAccepted = isAccepted;
        
        await _householdInvitesRepository.UpdateHouseholdInvite(invite);
    }
}