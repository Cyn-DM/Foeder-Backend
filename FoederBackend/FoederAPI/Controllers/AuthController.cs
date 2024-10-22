using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
using FoederDomain.DomainModels;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(Response credentialResponse)
        {
            try
            {
                var user = await _authService.Login(credentialResponse.CredentialResponse);

                if (user == null)
                {
                    return Unauthorized();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            
        }

    }

}

public class Response
{
    public required string CredentialResponse { get; set; }
}