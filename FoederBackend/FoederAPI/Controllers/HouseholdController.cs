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
    
    [HttpPost("AddHousehold")]
    public async Task<IActionResult> AddHousehold(Household household)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(household, new ValidationContext(household), results);
    
        if (results.Any())
        {
            return BadRequest(results.Select(m => m.ErrorMessage).ToList());
        }

        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString();
        var validationResult = await _householdService.AddHousehold(household, bearerToken);
    
        if (validationResult.ValidationResults.Count != 0)
        {
            return BadRequest(validationResult.ValidationResults.Select(m => m.ErrorMessage).ToList());
        }

        if (!validationResult.hasOperationSucceeded)
        {
            return StatusCode(500);
        }

        return Ok();


    }

    [HttpGet("GetHouseholdByUser")]
    public async Task<IActionResult> GetHouseholdByUserId(Guid userId)
    {
        var household = await _householdService.GetHouseholdByUserId(userId);

        if (household == null)
        {
            return NotFound();
        }

        return Ok(household);
    }
}