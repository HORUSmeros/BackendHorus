using BackendHorus.Data;
using BackendHorus.Dto;
using BackendHorus.Models;
using BackendHorus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Services.Implementations
{
    public class RecolectoresService : IRecolectoresService
    {
        private readonly MicrorutasDbContext _context;

        public RecolectoresService(MicrorutasDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecolectorStatusDto>> GetStatusAsync(int? macrorutaId)
        {
            // Tomamos el último trip de cada recolector
            var query = _context.Trips
                .Include(t => t.Recolector)
                .Include(t => t.Microruta).ThenInclude(m => m.Macroruta)
                .Include(t => t.Positions)
                .AsQueryable();

            if (macrorutaId.HasValue)
            {
                query = query.Where(t => t.Microruta.MacrorutaId == macrorutaId.Value);
            }

            var trips = await query
                .OrderByDescending(t => t.Inicio)
                .ToListAsync();

            // agrupar por recolector, tomar el más reciente
            var latestByRecolector = trips
                .GroupBy(t => t.RecolectorId)
                .Select(g => g.First())
                .ToList();

            var result = new List<RecolectorStatusDto>();

            foreach (var trip in latestByRecolector)
            {
                var lastPosition = trip.Positions
                    .OrderByDescending(p => p.Timestamp)
                    .FirstOrDefault();

                // lógica simple para el estado
                string estado;
                if (trip.Estado == "EnCurso")
                {
                    if (lastPosition != null && (DateTime.UtcNow - lastPosition.Timestamp).TotalMinutes > 5)
                        estado = "Sin señal";
                    else
                        estado = "En ruta";
                }
                else
                {
                    estado = "Completado";
                }

                result.Add(new RecolectorStatusDto
                {
                    RecolectorId = trip.RecolectorId,
                    RecolectorNombre = trip.Recolector.Nombre,
                    MicrorutaId = trip.MicrorutaId,
                    MicrorutaNombre = trip.Microruta.Nombre,
                    Estado = estado,
                    UltimaActualizacion = lastPosition?.Timestamp,
                    Bolsas = trip.Bolsas,
                    CoberturaPorciento = trip.CoberturaPorciento,
                    Latitude = lastPosition?.Latitude,
                    Longitude = lastPosition?.Longitude
                });
            }

            return result;
        }

        public async Task<IEnumerable<Recolector>> GetAllAsync()
        {
            return await _context.Recolectores
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }

        public async Task<Recolector?> GetByIdAsync(int id)
        {
            return await _context.Recolectores
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recolector> CreateAsync(Recolector recolector)
        {
            _context.Recolectores.Add(recolector);
            await _context.SaveChangesAsync();
            return recolector;
        }
    }
}
