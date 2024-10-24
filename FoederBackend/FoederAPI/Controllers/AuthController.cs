using FoederBusiness.Interfaces;
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
                var tokenResult = await _authService.Login(credentialResponse.CredentialResponse);

                if (tokenResult == null)
                {
                    return Unauthorized();
                }
                
                return Ok(tokenResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> Refresh(string refreshToken)
        {
            try
            {
                var result = await _authService.Refresh(refreshToken);

                if (!result!.isRefreshTokenFound)
                {
                    return StatusCode(500);
                }

                if (result.IsRefreshTokenExpired)
                {
                    return Unauthorized();
                }

                return Ok(result.AccessToken);
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