using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColorConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Récupère le profil de l'utilisateur authentifié.
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult GetProfile()

        {
            var claims = HttpContext.User.Claims.ToList();
            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return Ok(new
            {
                Username = username,
                Email = email
            });
        }
    }
}
