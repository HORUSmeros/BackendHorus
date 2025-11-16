using BackendHorus.Data;
using BackendHorus.Dto;
using BackendHorus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecolectorRutasController : ControllerBase
    {
        private readonly MicrorutasDbContext _context;

        public RecolectorRutasController(MicrorutasDbContext context)
        {
            _context = context;
        }

        // GET: api/RecolectorRutas
        // Lista todas las asignaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecolectorRutaDto>>> GetAll()
        {
            var query = await _context.RecolectorRutas
                .Include(r => r.Macroruta)
                .Include(r => r.Microruta)
                .ToListAsync();

            var result = query.Select(r => new RecolectorRutaDto
            {
                Id = r.Id,
                UserId = r.UserId,
                MacrorutaId = r.MacrorutaId,
                MacrorutaNombre = r.Macroruta.Nombre,
                MicrorutaId = r.MicrorutaId,
                MicrorutaNombre = r.Microruta.Nombre
            });

            return Ok(result);
        }

        // GET: api/RecolectorRutas/by-user/{userId}
        // Devuelve la(s) ruta(s) asignada(s) a un usuario concreto
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<RecolectorRutaDto>>> GetByUser(string userId)
        {
            var query = await _context.RecolectorRutas
                .Where(r => r.UserId == userId)
                .Include(r => r.Macroruta)
                .Include(r => r.Microruta)
                .ToListAsync();

            var result = query.Select(r => new RecolectorRutaDto
            {
                Id = r.Id,
                UserId = r.UserId,
                MacrorutaId = r.MacrorutaId,
                MacrorutaNombre = r.Macroruta.Nombre,
                MicrorutaId = r.MicrorutaId,
                MicrorutaNombre = r.Microruta.Nombre
            });

            return Ok(result);
        }

        // POST: api/RecolectorRutas
        // Crea una nueva asignación de macroruta/microruta para un usuario
        [HttpPost]
        public async Task<ActionResult<RecolectorRutaDto>> Create([FromBody] CreateRecolectorRutaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar que existan macroruta y microruta
            var macroruta = await _context.Macrorutas.FindAsync(dto.MacrorutaId);
            var microruta = await _context.Microrutas.FindAsync(dto.MicrorutaId);

            if (macroruta == null || microruta == null)
                return BadRequest("Macroruta o microruta no encontrada.");

            var entity = new RecolectorRuta
            {
                UserId = dto.UserId,
                MacrorutaId = dto.MacrorutaId,
                MicrorutaId = dto.MicrorutaId
            };

            _context.RecolectorRutas.Add(entity);
            await _context.SaveChangesAsync();

            var result = new RecolectorRutaDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                MacrorutaId = entity.MacrorutaId,
                MacrorutaNombre = macroruta.Nombre,
                MicrorutaId = entity.MicrorutaId,
                MicrorutaNombre = microruta.Nombre
            };

            return CreatedAtAction(nameof(GetByUser), new { userId = dto.UserId }, result);
        }

        // PUT: api/RecolectorRutas/{id}
        // Actualiza la asignación existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RecolectorRutaDto>> Update(int id, [FromBody] CreateRecolectorRutaDto dto)
        {
            var entity = await _context.RecolectorRutas.FindAsync(id);
            if (entity == null)
                return NotFound();

            var macroruta = await _context.Macrorutas.FindAsync(dto.MacrorutaId);
            var microruta = await _context.Microrutas.FindAsync(dto.MicrorutaId);

            if (macroruta == null || microruta == null)
                return BadRequest("Macroruta o microruta no encontrada.");

            entity.UserId = dto.UserId;
            entity.MacrorutaId = dto.MacrorutaId;
            entity.MicrorutaId = dto.MicrorutaId;

            await _context.SaveChangesAsync();

            var result = new RecolectorRutaDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                MacrorutaId = entity.MacrorutaId,
                MacrorutaNombre = macroruta.Nombre,
                MicrorutaId = entity.MicrorutaId,
                MicrorutaNombre = microruta.Nombre
            };

            return Ok(result);
        }

        // DELETE: api/RecolectorRutas/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.RecolectorRutas.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.RecolectorRutas.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
