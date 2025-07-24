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
        /// Retourne le profil de l'utilisateur connecté.
        /// </summary>
        /// <remarks>
        /// Nécessite un token JWT valide.
        /// Retourne le nom d'utilisateur et l'email extraits des claims.
        /// </remarks>
        /// <returns>Objet contenant le nom d'utilisateur et l'email</returns>
        /// <response code="200">Profil récupéré avec succès</response>
        /// <response code="401">Utilisateur non authentifié</response>
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
