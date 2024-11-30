using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.Data.SqlClient;

namespace FoederBusiness.Services;

public class HouseholdInvitesService : IHouseholdInvitesService
{
    private readonly IHouseholdInvitesRepository _householdInvitesRepository;
    private readonly IAuthRepository _authRepository;

    public HouseholdInvitesService(IHouseholdInvitesRepository householdInvitesRepository, IAuthRepository authRepository)
    {
        _householdInvitesRepository = householdInvitesRepository;
        _authRepository = authRepository;
    }
    public async Task<List<HouseholdInvite>> GetHouseholdInvites(Guid userId)
    {
        if (await _authRepository.FindUserById(userId) == null)
        {
            throw new UserNotFoundException();
        }
        return await _householdInvitesRepository.GetHouseholdInvites(userId);
    }

    public async Task<ValidationDTO> InviteToHousehold(HouseholdInvite householdInvite)
    {
        var validation = new ValidationDTO();

        try
        {
            var user = await _authRepository.FindUserByEmail(householdInvite.InvitedUser.Email);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            await _householdInvitesRepository.InviteToHousehold(householdInvite);

            validation.hasOperationSucceeded = true;
            return validation;

        }
        catch (SqlException ex)
        {
            validation.hasOperationSucceeded = false;
            validation.ValidationResults.Add(new ValidationResult(ex.Message));
            return validation;
        }
    }

    public async Task RespondToHouseholdInvite(HouseholdInvite householdInvite)
    {
        await _householdInvitesRepository.UpdateHouseholdInvite(householdInvite);
    }
}