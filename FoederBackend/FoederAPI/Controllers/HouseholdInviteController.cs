using System.ComponentModel.DataAnnotations;
using FoederBusiness.Interfaces;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL.Repository;
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
            return NotFound(new {message = ex.Message});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {message = ex.Message});
        }
    }

    [HttpPost("PostHouseholdInvite")]
    public async Task<IActionResult> PostHouseholdInvite([FromBody]InviteRequest inviteRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString();

        try
        {
            var householdId = JwtAuthTokenUtils.GetUserHouseholdIdFromToken(bearerToken);

            if (householdId == null)
            {
                return Unauthorized();
            }

            if (householdId != inviteRequest.HouseholdId)
            {
                return BadRequest("You cannot invite someone to a household you do not belong to.");
            }

        }
        catch (InvalidHouseholdIdException ex)
        {
            return BadRequest(ex.Message);
        }


        try
        {
            await _householdInvitesService.InviteToHousehold(inviteRequest.Email, inviteRequest.HouseholdId);

            return Ok();
        }
        catch (UserAlreadyHasHouseholdException ex)
        {
            return StatusCode(409, new {message = ex.Message});
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (HouseholdNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (SqlException ex)
        {
            return StatusCode(500, new {message = ex.Message});
        }
    }

    [HttpPost("RespondToHouseholdInvite")]
    public async Task<IActionResult> RespondToHouseholdInvite(InviteResponse inviteResponse)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (inviteResponse.IsAccepted == null)
        {
            return BadRequest("Household invite does not have accepted values");
        }

        try
        {
            await _householdInvitesService.RespondToHouseholdInvite(inviteResponse.Id,
                inviteResponse.IsAccepted.Value);
            return Ok();
        }
        catch (InviteNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (SqlException ex)
        {
            return StatusCode(500, new {message = ex.Message});
        }
    }

    public class InviteRequest()
    {
        public string Email { get; set; }
        public Guid HouseholdId { get; set; }
    }

    public class InviteResponse()
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool? IsAccepted { get; set; } 
    }
}