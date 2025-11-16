using System;

namespace BackendHorus.Dto
{
    public class RecolectorStatusDto
    {
        public int RecolectorId { get; set; }
        public string RecolectorNombre { get; set; } = null!;

        public int? MicrorutaId { get; set; }
        public string? MicrorutaNombre { get; set; }

        // 🟢 En ruta / 🔴 Fuera de ruta / 🟡 Sin señal
        public string Estado { get; set; } = null!;

        public DateTime? UltimaActualizacion { get; set; }

        // Extras útiles para el panel derecho
        public int Bolsas { get; set; }
        public double CoberturaPorciento { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}