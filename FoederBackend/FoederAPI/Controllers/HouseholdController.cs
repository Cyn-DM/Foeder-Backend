using System.ComponentModel.DataAnnotations;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederDomain.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HouseholdController : ControllerBase
{
    private readonly IHouseholdService _householdService;

    public HouseholdController(IHouseholdService householdService)
    {
        _householdService = householdService;
    }
    
    [HttpPost("addHousehold")]
    public async Task<IActionResult> AddHousehold([FromBody] Household household)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(household, new ValidationContext(household), results);

        if (!results.Any())
        {
            return BadRequest(results);
        }
        
        var validationResult = await _householdService.AddHousehold(household);
    
        if (validationResult.ValidationResults.Count == 0)
        {
            return BadRequest(validationResult.ValidationResults);
        }

        if (!validationResult.hasOperationSucceeded)
        {
            return StatusCode(500);
        }

        return Ok();


    }
}