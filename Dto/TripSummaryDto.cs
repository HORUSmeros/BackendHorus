using System;

namespace BackendHorus.Dto
{
    public class TripSummaryDto
    {
        public int Id { get; set; }

        public string RecolectorNombre { get; set; } = null!;
        public string MicrorutaNombre { get; set; } = null!;

        public DateTime Inicio { get; set; }
        public DateTime? Fin { get; set; }

        public int Bolsas { get; set; }
        public double CoberturaPorciento { get; set; }
        public double DistanciaMetros { get; set; }

        // En progreso / Completada / Incompleta
        public string Estado { get; set; } = null!;

        // Cantidad de incidentes detectados en el recorrido
        public int Incidentes { get; set; }

        // Propiedad calculada para usar en el front si quieren
        public double DuracionMinutos =>
            Fin.HasValue ? (Fin.Value - Inicio).TotalMinutes : 0;
    }
}