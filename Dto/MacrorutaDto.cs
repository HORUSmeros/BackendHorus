using System.Collections.Generic;

namespace BackendHorus.Dto
{
    public class MacrorutaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        // Para pintar en el mapa o en el selector
        public string ColorHex { get; set; } = "#4CAF50";

        // Por si quieren mostrar cuántas microrutas tiene
        public int MicrorutasCount { get; set; }
    }
}