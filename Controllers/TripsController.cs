using Microsoft.AspNetCore.Mvc;
using BackendHorus.Dto;
using BackendHorus.Services.Interfaces;
using BackendHorus.Models;

namespace BackendHorus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    // GET: api/trips?macrorutaId=1&date=2025-11-15
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripSummaryDto>>> GetTrips(
        [FromQuery] int? macrorutaId,
        [FromQuery] DateTime? date)
    {
        var result = await _tripsService.GetTripsAsync(macrorutaId, date);
        return Ok(result);
    }

    // GET: api/trips/activos?macrorutaId=1
    [HttpGet("activos")]
    public async Task<ActionResult<IEnumerable<TripSummaryDto>>> GetActiveTrips(
        [FromQuery] int? macrorutaId)
    {
        var result = await _tripsService.GetActiveTripsAsync(macrorutaId);
        return Ok(result);
    }

    // GET: api/trips/5
    [HttpGet("{id:int}", Name = "GetTripById")]
    public async Task<ActionResult<Trip>> GetTripById(int id)
    {
        var trip = await _tripsService.GetTripByIdAsync(id);
        if (trip == null) return NotFound();

        return Ok(trip);
    }

    // GET: api/trips/5/positions
    [HttpGet("{id:int}/positions")]
    public async Task<ActionResult<IEnumerable<PositionSample>>> GetTripPositions(int id)
    {
        var positions = await _tripsService.GetTripPositionsAsync(id);
        if (positions == null) return NotFound();

        return Ok(positions);
    }
}