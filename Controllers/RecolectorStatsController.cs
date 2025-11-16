using BackendHorus.Data;
using BackendHorus.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecolectorStatsController : ControllerBase
    {
        private readonly MicrorutasDbContext _context;
        private readonly Random _random = new();

        public RecolectorStatsController(MicrorutasDbContext context)
        {
            _context = context;
        }

        // GET: api/RecolectorStats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecolectorStatsDto>>> GetAll()
        {
            var stats = await _context.RecolectorStats
                .Include(s => s.Recolector)
                .Select(s => new RecolectorStatsDto
                {
                    Id = s.Id,
                    RecolectorId = s.RecolectorId,
                    RecolectorNombre = s.Recolector.Nombre,
                    BolsasTotales = s.BolsasTotales,
                    CoberturaPromedio = s.CoberturaPromedio,
                    CantidadRecorridos = s.CantidadRecorridos
                })
                .ToListAsync();

            return Ok(stats);
        }

        // GET: api/RecolectorStats/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecolectorStatsDto>> GetById(int id)
        {
            var s = await _context.RecolectorStats
                .Include(x => x.Recolector)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (s == null)
                return NotFound();

            var dto = new RecolectorStatsDto
            {
                Id = s.Id,
                RecolectorId = s.RecolectorId,
                RecolectorNombre = s.Recolector.Nombre,
                BolsasTotales = s.BolsasTotales,
                CoberturaPromedio = s.CoberturaPromedio,
                CantidadRecorridos = s.CantidadRecorridos
            };

            return Ok(dto);
        }

        // GET: api/RecolectorStats/by-recolector/{recolectorId}
        [HttpGet("by-recolector/{recolectorId:int}")]
        public async Task<ActionResult<RecolectorStatsDto>> GetByRecolector(int recolectorId)
        {
            var s = await _context.RecolectorStats
                .Include(x => x.Recolector)
                .FirstOrDefaultAsync(x => x.RecolectorId == recolectorId);

            if (s == null)
                return NotFound();

            var dto = new RecolectorStatsDto
            {
                Id = s.Id,
                RecolectorId = s.RecolectorId,
                RecolectorNombre = s.Recolector.Nombre,
                BolsasTotales = s.BolsasTotales,
                CoberturaPromedio = s.CoberturaPromedio,
                CantidadRecorridos = s.CantidadRecorridos
            };

            return Ok(dto);
        }

        // POST: api/RecolectorStats
        // Crea stats nuevas (o regenera) para un recolector, con valores random.
        [HttpPost]
        public async Task<ActionResult<RecolectorStatsDto>> CreateOrRegenerate([FromBody] CreateRecolectorStatsDto request)
        {
            var recolector = await _context.Recolectores.FindAsync(request.RecolectorId);
            if (recolector == null)
            {
                return BadRequest($"No existe un recolector con id {request.RecolectorId}");
            }

            // Buscar si ya hay stats para este recolector
            var existing = await _context.RecolectorStats
                .FirstOrDefaultAsync(x => x.RecolectorId == request.RecolectorId);

            // Generar valores random:
            // BolsasTotales: >= 100, por ejemplo entre 100 y 500
            var bolsasTotales = _random.Next(100, 501);

            // CoberturaPromedio: porcentaje entre 60 y 100
            var coberturaPromedio = _random.NextDouble() * 40 + 60; // [60,100)

            // CantidadRecorridos: por ejemplo entre 5 y 50
            var cantidadRecorridos = _random.Next(5, 51);

            if (existing == null)
            {
                var stats = new RecolectorStats
                {
                    RecolectorId = request.RecolectorId,
                    BolsasTotales = bolsasTotales,
                    CoberturaPromedio = coberturaPromedio,
                    CantidadRecorridos = cantidadRecorridos
                };

                _context.RecolectorStats.Add(stats);
                await _context.SaveChangesAsync();

                var dto = new RecolectorStatsDto
                {
                    Id = stats.Id,
                    RecolectorId = stats.RecolectorId,
                    RecolectorNombre = recolector.Nombre,
                    BolsasTotales = stats.BolsasTotales,
                    CoberturaPromedio = stats.CoberturaPromedio,
                    CantidadRecorridos = stats.CantidadRecorridos
                };

                return CreatedAtAction(nameof(GetById), new { id = stats.Id }, dto);
            }
            else
            {
                // Si ya existe, actualizamos con nuevos randoms
                existing.BolsasTotales = bolsasTotales;
                existing.CoberturaPromedio = coberturaPromedio;
                existing.CantidadRecorridos = cantidadRecorridos;

                await _context.SaveChangesAsync();

                var dto = new RecolectorStatsDto
                {
                    Id = existing.Id,
                    RecolectorId = existing.RecolectorId,
                    RecolectorNombre = recolector.Nombre,
                    BolsasTotales = existing.BolsasTotales,
                    CoberturaPromedio = existing.CoberturaPromedio,
                    CantidadRecorridos = existing.CantidadRecorridos
                };

                return Ok(dto);
            }
        }
    }
}
