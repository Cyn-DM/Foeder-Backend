using System.Net;
using FoederBusiness.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace FoederAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
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
                    Expires = DateTimeOffset.Now.AddDays(7),
                    Domain = "localhost",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                    IsEssential = true,
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

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized();
            }
            
            var decodedToken = HttpUtility.UrlDecode(refreshToken);
            
            try
            {
                var result = await authService.Refresh(decodedToken);

                if (!result!.isRefreshTokenFound)
                {
                    RemoveRefreshTokenCookie(HttpContext);
                    return StatusCode(500);
                }

                if (result.IsRefreshTokenExpired)
                {
                    RemoveRefreshTokenCookie(HttpContext);
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

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            if (RemoveRefreshTokenCookie(HttpContext))
            {
                return Ok();
            }

            return NotFound();
        }

        [ApiExplorerSettings(IgnoreApi=true)]
        public bool RemoveRefreshTokenCookie(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies["refreshToken"] != null)
            {
                httpContext.Response.Cookies.Delete("refreshToken");
                return true;
            }

            return false;
        }
        
    }
    public class Response
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public required string CredentialResponse { get; set; }
    }
}


