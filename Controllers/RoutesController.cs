using Microsoft.AspNetCore.Mvc;
using BackendHorus.Dto;
using BackendHorus.Services.Interfaces;

namespace BackendHorus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly IRutasService _rutasService;

    public RoutesController(IRutasService rutasService)
    {
        _rutasService = rutasService;
    }

    // GET: api/routes/macrorutas
    [HttpGet("macrorutas")]
    public async Task<ActionResult<IEnumerable<MacrorutaDto>>> GetMacrorutas()
    {
        var result = await _rutasService.GetMacrorutasAsync();
        return Ok(result);
    }

    // GET: api/routes/microrutas?macrorutaId=1
    [HttpGet("microrutas")]
    public async Task<ActionResult<IEnumerable<MicrorutaDto>>> GetMicrorutas([FromQuery] int? macrorutaId)
    {
        var result = await _rutasService.GetMicrorutasAsync(macrorutaId);
        return Ok(result);
    }

    // GET: api/routes/microrutas/5
    [HttpGet("microrutas/{id:int}")]
    public async Task<ActionResult<MicrorutaDto>> GetMicrorutaById(int id)
    {
        var microruta = await _rutasService.GetMicrorutaByIdAsync(id);

        if (microruta == null)
            return NotFound();

        return Ok(microruta);
    }
}