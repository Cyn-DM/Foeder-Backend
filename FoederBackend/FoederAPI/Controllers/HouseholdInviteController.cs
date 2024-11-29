using FoederBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers;

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
            var invites = _householdInvitesService.GetHouseholdInvites(userId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
        
        
        
        
        
    }
}