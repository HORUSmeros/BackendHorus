using System.Collections.Generic;

namespace BackendHorus.Models
{
    public class Microruta
    {
        public int Id { get; set; }

        // Ej: "Microruta 1A", "Zona Azul 3"
        public string Nombre { get; set; } = null!;

        public int MacrorutaId { get; set; }
        public Macroruta Macroruta { get; set; } = null!;

        /// <summary>
        /// Ruta ideal de la microruta, guardada como JSON (lista de puntos lat/lon).
        /// Ej: [{ "lat": -17.78, "lng": -63.18 }, ...]
        /// El RutasService la deserializa y la mapea a MicrorutaDto.Points.
        /// </summary>
        public string PolylineJson { get; set; } = "[]";

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}