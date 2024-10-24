using FoederBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoederAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> LogIn(Response credentialResponse)
        {
            try
            {
                var tokenResult = await authService.Login(credentialResponse.CredentialResponse);

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

        [HttpPost]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            try
            {
                var result = await authService.Refresh(refreshToken);

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
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public required string CredentialResponse { get; set; }
}