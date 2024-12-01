using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederDomain.CustomExceptions;
using FoederDomain.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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

    [HttpGet("GetHouseholdInvites")]
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

    [HttpPost("PostHouseholdInvite")]
    public async Task<IActionResult> PostHouseholdInvite([FromBody]InviteRequest inviteRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var validation = await _householdInvitesService.InviteToHousehold(inviteRequest.Email, inviteRequest.HouseholdId);

            if (validation.ValidationResults.Count > 0)
            {
                return BadRequest(validation.ValidationResults);
            }

            return !validation.hasOperationSucceeded ? StatusCode(500) : Ok();
        }
        catch (UserAlreadyHasHouseholdException ex)
        {
            return StatusCode(409, ex.Message);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (HouseholdNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (SqlException ex)
        {
            return StatusCode(500, ex);
        }
    }

    [HttpPost("RespondToHouseholdInvite")]
    public async Task<IActionResult> RespondToHouseholdInvite(HouseholdInvite householdInvite)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (householdInvite.IsAccepted == null)
        {
            return BadRequest("Household invite does not have accepted values");
        }

        try
        {
            await _householdInvitesService.RespondToHouseholdInvite(householdInvite);
            return Ok();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (SqlException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    public class InviteRequest()
    {
        public string Email { get; set; }
        public Guid HouseholdId { get; set; }
    }
}