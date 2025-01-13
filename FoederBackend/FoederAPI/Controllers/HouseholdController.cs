using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> AddHousehold(Household household)
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString();

        try
        {
            await _householdService.AddHousehold(household, bearerToken);
        }
        catch (InvalidObjectException ex)
        {
            return BadRequest(ex.ValidationResults.Select(m => m.ErrorMessage).ToList());
        }
        catch (InvalidBearerTokenException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UserAlreadyHasHouseholdException ex)
        {
            return Conflict(ex.Message);
        }

        return Ok();


    }

    [HttpGet("GetHouseholdByUser")]
    [Authorize]
    public async Task<IActionResult> GetHouseholdByUserId(Guid userId)
    {
        var household = await _householdService.GetHouseholdByUserId(userId);

        if (household == null)
        {
            return NotFound();
        }

        return Ok(household);
    }

    [HttpDelete("LeaveHousehold")]
    [Authorize]
    public async Task<IActionResult> LeaveHousehold(Guid userId)
    {
        try
        {
            await _householdService.LeaveHousehold(userId);
            return Ok();
        }
        catch (HouseholdNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {message = ex.Message});
        }
    }
}