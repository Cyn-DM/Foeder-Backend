using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using FoederDomain.Interfaces;
using Microsoft.Data.SqlClient;

namespace FoederBusiness.Services;

public class HouseholdService : IHouseholdService
{
    private readonly IHouseholdRepository _householdRepository;

    public HouseholdService(IHouseholdRepository householdRepository)
    {
        _householdRepository = householdRepository;
    }
    public async Task<ValidationDTO> AddHousehold(Household household)
    {
        var dto = ValidationUtils.ValidateObject(household);

        try
        {
            await _householdRepository.AddHousehold(household);
            dto.hasOperationSucceeded = true;
        }
        catch (SqlException e)
        {
            dto.hasOperationSucceeded = false;
        }

        return dto;

    }
}