using ColorConverter.Data;
using ColorConverter.DTOs;
using ColorConverter.Models;
using ColorConverter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Convertit une couleur d’un format à un autre.
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>
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
        /// Retourne l’historique des conversions de couleurs.
        /// </summary>
        /// <returns></returns>
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
        /// Supprime une entrée de l’historique des conversions.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
