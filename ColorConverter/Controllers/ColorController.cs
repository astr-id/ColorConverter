using ColorConverter.Data;
using ColorConverter.DTOs;
using ColorConverter.Models;
using ColorConverter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorConverter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _service;
        private readonly ColorContext _context;

        public ColorController(IColorService service, ColorContext context)
        {
            _service = service;
            _context = context;
        }

        /// <summary>
        /// Convertit une couleur d’un format vers un autre.
        /// </summary>
        /// <param name="request">Objet contenant la couleur et les formats</param>
        /// <returns>La couleur convertie</returns>
        /// <response code="200">Conversion réussie</response>
        /// <response code="400">Format non supporté ou données invalides</response>
        [HttpPost("convert")]
        [Authorize]
        [ProducesResponseType(typeof(ColorResponseDto), 200)]
        [ProducesResponseType(400)]
        public IActionResult ConvertColor([FromBody] ColorRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = _service.ConvertColor(request);
                return Ok(result);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Récupère l’historique des conversions enregistrées.
        /// </summary>
        /// <returns>Liste des conversions</returns>
        /// <response code="200">Historique récupéré avec succès</response>
        [HttpGet("history")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ColorConversion>), 200)]
        public IActionResult GetHistory()
        {
            var history = _context.Conversions
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return Ok(history);
        }

        /// <summary>
        /// Supprime une conversion de l’historique.
        /// </summary>
        /// <param name="id">Identifiant de la conversion</param>
        /// <returns>Statut de la suppression</returns>
        /// <response code="200">Suppression réussie</response>
        /// <response code="404">Conversion non trouvée</response>
        [HttpDelete("history/{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            var item = _context.Conversions.Find(id);
            if (item == null)
                return NotFound();

            _context.Conversions.Remove(item);
            _context.SaveChanges();
            return Ok();
        }
    }
}
