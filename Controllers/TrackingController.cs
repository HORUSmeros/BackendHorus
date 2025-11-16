using Microsoft.AspNetCore.Mvc;
using BackendHorus.Dto;
using BackendHorus.Services.Interfaces;
using BackendHorus.Models;

namespace BackendHorus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackingController : ControllerBase
{
    private readonly ITrackingService _trackingService;

    public TrackingController(ITrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    // POST: api/tracking/start
    [HttpPost("start")]
    public async Task<ActionResult<Trip>> StartTrip([FromBody] StartTripDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var trip = await _trackingService.StartTripAsync(dto);
        return CreatedAtRoute(
            routeName: "GetTripById",
            routeValues: new { id = trip.Id },
            value: trip
        );
    }

    // POST: api/tracking/finish
    [HttpPost("finish")]
    public async Task<ActionResult> FinishTrip([FromBody] FinishTripDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var trip = await _trackingService.FinishTripAsync(dto);
        if (trip == null) return NotFound();

        return Ok(trip);
    }

    // POST: api/tracking/position
    [HttpPost("position")]
    public async Task<ActionResult> RegisterPosition([FromBody] PositionUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ok = await _trackingService.RegisterPositionAsync(dto);
        if (!ok) return NotFound(); // por ejemplo, Trip no existe

        return Ok();
    }

    // POST: api/tracking/bag
    [HttpPost("bag")]
    public async Task<ActionResult> RegisterBag([FromBody] BagEventDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ok = await _trackingService.RegisterBagAsync(dto);
        if (!ok) return NotFound();

        return Ok();
    }

    // POST: api/tracking/incident
    [HttpPost("incident")]
    public async Task<ActionResult> RegisterIncident([FromBody] IncidentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ok = await _trackingService.RegisterIncidentAsync(dto);
        if (!ok) return NotFound();

        return Ok();
    }
}
