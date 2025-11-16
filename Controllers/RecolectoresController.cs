using Microsoft.AspNetCore.Mvc;
using BackendHorus.Dto;
using BackendHorus.Models;
using BackendHorus.Services.Interfaces;

namespace BackendHorus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecolectoresController : ControllerBase
{
    private readonly IRecolectoresService _recolectoresService;

    public RecolectoresController(IRecolectoresService recolectoresService)
    {
        _recolectoresService = recolectoresService;
    }

    // GET: api/recolectores/status?macrorutaId=1
    // Lista para la tabla lateral de "Recolectores"
    [HttpGet("status")]
    public async Task<ActionResult<IEnumerable<RecolectorStatusDto>>> GetStatus(
        [FromQuery] int? macrorutaId)
    {
        var result = await _recolectoresService.GetStatusAsync(macrorutaId);
        return Ok(result);
    }

    // GET: api/recolectores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recolector>>> GetAll()
    {
        var result = await _recolectoresService.GetAllAsync();
        return Ok(result);
    }

    // POST: api/recolectores
    // Para crear recolectores de prueba rápido
    [HttpPost]
    public async Task<ActionResult<Recolector>> Create([FromBody] Recolector recolector)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _recolectoresService.CreateAsync(recolector);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // GET: api/recolectores/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Recolector>> GetById(int id)
    {
        var recolector = await _recolectoresService.GetByIdAsync(id);
        if (recolector == null) return NotFound();

        return Ok(recolector);
    }
}