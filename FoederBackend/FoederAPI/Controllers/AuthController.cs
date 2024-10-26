using System.Net;
using FoederBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace FoederAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        
        public async Task<IActionResult> LogIn(Response credentialResponse)
        {
            try
            {
                var tokenResult = await authService.Login(credentialResponse.CredentialResponse);

                if (tokenResult == null)
                {
                    return Unauthorized();
                }

                var cookieOptions = new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                    Domain = "localhost",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                };
                
                HttpContext.Response.Cookies.Append("refreshToken", tokenResult.RefreshToken, cookieOptions);
                
                return Ok(tokenResult.AccessToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("refresh")]
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