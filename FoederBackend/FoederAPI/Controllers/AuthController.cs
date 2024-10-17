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
        public async Task<IActionResult> VerifyGoogleIdToken([FromBody] string idToken)
        {
            try
            {
                TokenVerificationResult result = await _authService.VerifyGoogleIdToken(idToken);

                if (!result.isValid)
                {
                    return Unauthorized(result.errorMessage);
                }

                return Ok(result.payload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            
        }
    }


}
