using System;
using System.Collections.Generic;

namespace BackendHorus.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public int RecolectorId { get; set; }
        public Recolector Recolector { get; set; } = null!;

        public int MicrorutaId { get; set; }
        public Microruta Microruta { get; set; } = null!;

        // ⏰ Inicio y fin del recorrido
        public DateTime Inicio { get; set; }
        public DateTime? Fin { get; set; }

        // Métricas principales
        public int Bolsas { get; set; }
        public double CoberturaPorciento { get; set; }
        public double DistanciaMetros { get; set; }

        // EnCurso / Completado / Incompleto
        public string Estado { get; set; } = "EnCurso";

        public ICollection<PositionSample> Positions { get; set; } = new List<PositionSample>();
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}