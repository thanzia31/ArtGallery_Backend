using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.DTO;
using ArtGallery_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArtGallery_Backend.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LoginService _loginService;
     

        public LoginController(AppDbContext context, LoginService loginService)
        {
            _context = context;
            _loginService = loginService;
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _loginService.Authenticate(loginDto.email, loginDto.password);
            var user = await _loginService.GetUserByEmailAndPassword(loginDto.email, loginDto.password);

            if (token == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            Response.Cookies.Append("AccessToken", token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                //SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30),
            });

            Response.Cookies.Append("RefreshToken", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                //SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            return Ok(new { message= "Logged In Successfully", role= user.RoleId });
        }

   [HttpGet("me")]
public async Task<IActionResult> GetMe()
{
    if (Request.Cookies.TryGetValue("AccessToken", out string token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var user = await _loginService.getUserById(int.Parse(userId));

        if (userId == null || role == null)
                    return Unauthorized(new { message = "Invalid token" });

                return Ok(new
                {
                    id = userId,
                    role = role,
                    username = user.FirstName
            
        });
    }

    return Unauthorized(new { message = "No valid session" });
}

        [HttpGet("checkLogin")]
        public IActionResult checkLogin()
        {
            if (Request.Cookies.TryGetValue("AccessToken", out string token))
            {
                try
                {
                    
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    
                    if (jwtToken.ValidTo > DateTime.UtcNow)
                    {
                        return Ok(new { loggedIn = true, userId = jwtToken.Subject });
                    }
                }
                catch
                {
                    // Invalid token
                    return Ok(new { loggedIn = false });
                }

            }
            return Ok(new { loggedIn = false });
        
}


        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var accessToken = Request.Cookies["AccessToken"];

            if(string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token found");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userId == null || role == null)
                return Unauthorized("Invalid token data.");

            var newAccessToken = _loginService.RefreshAccessToken(int.Parse(userId), role);

            Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true,
                //SameSite = SameSiteMode.Strict
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            return Ok("Access Token Refreshed.");
        }
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("AccessToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            Response.Cookies.Append("RefreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok(new { message = "Logged Out" });
        }

    }
}
