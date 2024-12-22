using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.Data.SqlClient;

namespace FoederBusiness.Services;

public class HouseholdService : IHouseholdService
{
    private readonly IHouseholdRepository _householdRepository;
    private readonly IAuthRepository _authRepository;

    public HouseholdService(IHouseholdRepository householdRepository, IAuthRepository authRepository)
    {
        _householdRepository = householdRepository;
        _authRepository = authRepository;
    }
    public async Task AddHousehold(Household household, string bearerToken)
    {
        ValidationUtils.ValidateObject(household);
        
        var email = JwtAuthTokenUtils.GetUserEmailFromToken(bearerToken);

        if (email == null)
        {
            throw new InvalidBearerTokenException();
        }
        
        var user = await _authRepository.FindUserByEmail(email);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Household != null)
        {
            throw new UserAlreadyHasHouseholdException();
        }
        
        await _householdRepository.AddHousehold(household, user);
    }

    public async Task<Household?> GetHouseholdByUserId(Guid userId)
    {
        var household = await _householdRepository.GetHouseholdByUserId(userId);
        
        return household;
    }

    public async Task LeaveHousehold(Guid userId)
    {
        var user = await _authRepository.FindUserById(userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var household = await _householdRepository.GetHouseholdByUserId(userId);
        if (household == null)
        {
            throw new HouseholdNotFoundException();
        }
        
        await _householdRepository.LeaveHousehold(household, user);
    }
}