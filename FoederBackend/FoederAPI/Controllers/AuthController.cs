using System.IdentityModel.Tokens.Jwt;
using FoederBusiness.Interfaces;
using FoederBusiness.Tools;
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
        public async Task<IActionResult> VerifyGoogleIdToken(Response credentialResponse)
        {
            try
            {
                TokenVerificationResult result =
                    await _authService.VerifyGoogleIdToken(credentialResponse.CredentialResponse);

                if (!result.isValid)
                {
                    return Unauthorized(result.errorMessage);
                }

                return Ok(result.payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            
        }

        //[HttpPost]
        //public async Task<IActionResult> LogIn(Response credentialResponse)
        //{

        //}
    }

}

public class Response
{
    public string CredentialResponse { get; set; }
}