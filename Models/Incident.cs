using System;

namespace BackendHorus.Models
{
    public class Incident
    {
        public int Id { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Ej: "Calle bloqueada", "Punto inaccesible", etc.
        public string Tipo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime Timestamp { get; set; }
    }
}