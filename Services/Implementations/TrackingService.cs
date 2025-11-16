using BackendHorus.Data;
using BackendHorus.Dto;
using BackendHorus.Hubs;
using BackendHorus.Models;
using BackendHorus.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BackendHorus.Services.Implementations
{
    public class TrackingService : ITrackingService
    {
        private readonly MicrorutasDbContext _context;
        private readonly IHubContext<TrackingHub> _hub;

        public TrackingService(MicrorutasDbContext context, IHubContext<TrackingHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<Trip> StartTripAsync(StartTripDto dto)
        {
            var inicio = dto.Inicio ?? DateTime.UtcNow;

            var trip = new Trip
            {
                RecolectorId = dto.RecolectorId,
                MicrorutaId = dto.MicrorutaId,
                Inicio = inicio,
                Estado = "EnCurso",
                Bolsas = 0,
                CoberturaPorciento = 0,
                DistanciaMetros = 0
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            // Notificar al dashboard que hay nuevo Trip
            await _hub.Clients
                .Group($"{TrackingHub.MicrorutaGroupPrefix}{trip.MicrorutaId}")
                .SendAsync("TripStarted", new
                {
                    tripId = trip.Id,
                    recolectorId = trip.RecolectorId,
                    microrutaId = trip.MicrorutaId,
                    inicio = trip.Inicio
                });

            return trip;
        }

        public async Task<Trip?> FinishTripAsync(FinishTripDto dto)
        {
            var trip = await _context.Trips.FindAsync(dto.TripId);
            if (trip == null) return null;

            var fin = dto.Fin ?? DateTime.UtcNow;
            trip.Fin = fin;

            if (trip.Estado == "EnCurso")
                trip.Estado = "Completado";

            await _context.SaveChangesAsync();

            await _hub.Clients
                .Group($"{TrackingHub.MicrorutaGroupPrefix}{trip.MicrorutaId}")
                .SendAsync("TripFinished", new
                {
                    tripId = trip.Id,
                    fin = trip.Fin,
                    estado = trip.Estado
                });

            return trip;
        }

        public async Task<bool> RegisterPositionAsync(PositionUpdateDto dto)
        {
            var trip = await _context.Trips
                .Include(t => t.Positions)
                .FirstOrDefaultAsync(t => t.Id == dto.TripId);

            if (trip == null) return false;

            var timestamp = dto.Timestamp ?? DateTime.UtcNow;

            // Guardar muestra de posición
            var sample = new PositionSample
            {
                TripId = trip.Id,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Timestamp = timestamp
            };

            _context.PositionSamples.Add(sample);

            // Calcular distancia aproximada desde la última muestra
            var last = trip.Positions.OrderByDescending(p => p.Timestamp).FirstOrDefault();
            if (last != null)
            {
                trip.DistanciaMetros += HaversineDistanceMeters(
                    last.Latitude, last.Longitude,
                    dto.Latitude, dto.Longitude);
            }

            await _context.SaveChangesAsync();

            // Notificar posición nueva
            await _hub.Clients
                .Group($"{TrackingHub.MicrorutaGroupPrefix}{trip.MicrorutaId}")
                .SendAsync("PositionUpdated", new
                {
                    tripId = trip.Id,
                    latitude = dto.Latitude,
                    longitude = dto.Longitude,
                    timestamp
                });

            return true;
        }

        public async Task<bool> RegisterBagAsync(BagEventDto dto)
        {
            var trip = await _context.Trips.FindAsync(dto.TripId);
            if (trip == null) return false;

            trip.Bolsas += 1;
            await _context.SaveChangesAsync();

            await _hub.Clients
                .Group($"{TrackingHub.MicrorutaGroupPrefix}{trip.MicrorutaId}")
                .SendAsync("BagsUpdated", new
                {
                    tripId = trip.Id,
                    bolsas = trip.Bolsas
                });

            return true;
        }

        public async Task<bool> RegisterIncidentAsync(IncidentDto dto)
        {
            var trip = await _context.Trips.FindAsync(dto.TripId);
            if (trip == null) return false;

            var timestamp = dto.Timestamp ?? DateTime.UtcNow;

            var incident = new Incident
            {
                TripId = trip.Id,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Tipo = dto.Tipo,
                Descripcion = dto.Descripcion,
                Timestamp = timestamp
            };

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            // Notificar incidente al dashboard
            await _hub.Clients
                .Group($"{TrackingHub.MicrorutaGroupPrefix}{trip.MicrorutaId}")
                .SendAsync("IncidentCreated", new
                {
                    tripId = trip.Id,
                    latitude = dto.Latitude,
                    longitude = dto.Longitude,
                    tipo = dto.Tipo,
                    descripcion = dto.Descripcion,
                    timestamp
                });

            return true;
        }

        // Distancia aproximada en metros usando Haversine
        private static double HaversineDistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // radio de la Tierra en m
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) *
                Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double DegreesToRadians(double deg) => deg * Math.PI / 180.0;
    }
}
