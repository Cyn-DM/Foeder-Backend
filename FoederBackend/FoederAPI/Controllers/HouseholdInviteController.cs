using FoederBusiness.Interfaces;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HouseholdInviteController : ControllerBase
{
    private readonly IHouseholdInvitesService _householdInvitesService;

    public HouseholdInviteController(IHouseholdInvitesService householdInvitesService)
    {
        _householdInvitesService = householdInvitesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetHouseholdInvites(Guid userId)
    {
        try
        {
            var invites = await _householdInvitesService.GetHouseholdInvites(userId);
            return Ok(invites);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostHouseholdInvite(HouseholdInvite householdInvite)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var validation = await _householdInvitesService.InviteToHousehold(householdInvite);

            if (validation.ValidationResults.Count > 0)
            {
                return BadRequest(validation.ValidationResults);
            }

            return !validation.hasOperationSucceeded ? StatusCode(500) : Ok();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
    
   
}