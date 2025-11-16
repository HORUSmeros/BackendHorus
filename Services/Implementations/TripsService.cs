using BackendHorus.Data;
using BackendHorus.Dto;
using BackendHorus.Models;
using BackendHorus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Services.Implementations
{
    public class TripsService : ITripsService
    {
        private readonly MicrorutasDbContext _context;

        public TripsService(MicrorutasDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TripSummaryDto>> GetTripsAsync(int? macrorutaId, DateTime? date)
        {
            var query = _context.Trips
                .Include(t => t.Recolector)
                .Include(t => t.Microruta).ThenInclude(m => m.Macroruta)
                .Include(t => t.Incidents)
                .AsQueryable();

            if (macrorutaId.HasValue)
            {
                query = query.Where(t => t.Microruta.MacrorutaId == macrorutaId.Value);
            }

            if (date.HasValue)
            {
                var day = date.Value.Date;
                var next = day.AddDays(1);
                query = query.Where(t => t.Inicio >= day && t.Inicio < next);
            }

            var list = await query.ToListAsync();

            return list.Select(t => new TripSummaryDto
            {
                Id = t.Id,
                RecolectorNombre = t.Recolector.Nombre,
                MicrorutaNombre = t.Microruta.Nombre,
                Inicio = t.Inicio,
                Fin = t.Fin,
                Bolsas = t.Bolsas,
                CoberturaPorciento = t.CoberturaPorciento,
                DistanciaMetros = t.DistanciaMetros,
                Estado = t.Estado,
                Incidentes = t.Incidents.Count
            });
        }

        public async Task<IEnumerable<TripSummaryDto>> GetActiveTripsAsync(int? macrorutaId)
        {
            var query = _context.Trips
                .Include(t => t.Recolector)
                .Include(t => t.Microruta).ThenInclude(m => m.Macroruta)
                .Include(t => t.Incidents)
                .Where(t => t.Estado == "EnCurso");

            if (macrorutaId.HasValue)
            {
                query = query.Where(t => t.Microruta.MacrorutaId == macrorutaId.Value);
            }

            var list = await query.ToListAsync();

            return list.Select(t => new TripSummaryDto
            {
                Id = t.Id,
                RecolectorNombre = t.Recolector.Nombre,
                MicrorutaNombre = t.Microruta.Nombre,
                Inicio = t.Inicio,
                Fin = t.Fin,
                Bolsas = t.Bolsas,
                CoberturaPorciento = t.CoberturaPorciento,
                DistanciaMetros = t.DistanciaMetros,
                Estado = t.Estado,
                Incidentes = t.Incidents.Count
            });
        }

        public async Task<Trip?> GetTripByIdAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.Recolector)
                .Include(t => t.Microruta).ThenInclude(m => m.Macroruta)
                .Include(t => t.Positions)
                .Include(t => t.Incidents)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<PositionSample>?> GetTripPositionsAsync(int tripId)
        {
            var exists = await _context.Trips.AnyAsync(t => t.Id == tripId);
            if (!exists) return null;

            return await _context.PositionSamples
                .Where(p => p.TripId == tripId)
                .OrderBy(p => p.Timestamp)
                .ToListAsync();
        }
    }
}
