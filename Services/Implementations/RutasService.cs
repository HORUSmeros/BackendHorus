using System.Text.Json;
using BackendHorus.Data;
using BackendHorus.Dto;
using BackendHorus.Models;
using BackendHorus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Services.Implementations
{
    public class RutasService : IRutasService
    {
        private readonly MicrorutasDbContext _context;

        public RutasService(MicrorutasDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MacrorutaDto>> GetMacrorutasAsync()
        {
            return await _context.Macrorutas
                .Select(m => new MacrorutaDto
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    ColorHex = m.ColorHex,
                    MicrorutasCount = m.Microrutas.Count
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MicrorutaDto>> GetMicrorutasAsync(int? macrorutaId)
        {
            var query = _context.Microrutas
                .Include(m => m.Macroruta)
                .AsQueryable();

            if (macrorutaId.HasValue)
                query = query.Where(m => m.MacrorutaId == macrorutaId.Value);

            var list = await query.ToListAsync();

            return list.Select(MapMicrorutaToDto);
        }

        public async Task<MicrorutaDto?> GetMicrorutaByIdAsync(int id)
        {
            var microruta = await _context.Microrutas
                .Include(m => m.Macroruta)
                .FirstOrDefaultAsync(m => m.Id == id);

            return microruta == null ? null : MapMicrorutaToDto(microruta);
        }

        private static MicrorutaDto MapMicrorutaToDto(Microruta microruta)
        {
            var points = new List<MicrorutaPointDto>();

            if (!string.IsNullOrWhiteSpace(microruta.PolylineJson))
            {
                try
                {
                    var raw = JsonSerializer.Deserialize<List<LatLngPoint>>(microruta.PolylineJson)
                              ?? new List<LatLngPoint>();

                    points = raw.Select(p => new MicrorutaPointDto
                    {
                        Latitude = p.lat,
                        Longitude = p.lng,
                        IsBlocked = false // para este hack no bloqueamos a nivel ruta ideal
                    }).ToList();
                }
                catch
                {
                    // si falla el JSON, devolvemos lista vacía
                }
            }

            return new MicrorutaDto
            {
                Id = microruta.Id,
                Nombre = microruta.Nombre,
                MacrorutaId = microruta.MacrorutaId,
                MacrorutaNombre = microruta.Macroruta?.Nombre ?? "",
                Points = points
            };
        }

        // clase auxiliar para deserializar el JSON
        private class LatLngPoint
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }
}
