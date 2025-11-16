using System;

namespace BackendHorus.Dto
{
    public class IncidentDto
    {
        public int TripId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Ej: "Calle bloqueada", "Obra", "Punto inaccesible"
        public string Tipo { get; set; } = null!;

        public string? Descripcion { get; set; }

        // Opcional; si null, el service usa la hora actual
        public DateTime? Timestamp { get; set; }
    }
}