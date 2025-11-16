using System.Collections.Generic;

namespace BackendHorus.Models
{
    public class Macroruta
    {
        public int Id { get; set; }

        // Ej: "Macroruta Verde", "Macroruta Roja"
        public string Nombre { get; set; } = null!;

        // Para que el front pueda pintar el color de la zona
        public string ColorHex { get; set; } = "#4CAF50";

        public ICollection<Microruta> Microrutas { get; set; } = new List<Microruta>();
    }
}