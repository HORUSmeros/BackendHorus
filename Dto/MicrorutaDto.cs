using System.Collections.Generic;

namespace BackendHorus.Dto
{
    public class MicrorutaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public int MacrorutaId { get; set; }
        public string MacrorutaNombre { get; set; } = null!;

        // Lista de puntos en orden de recorrido
        public List<MicrorutaPointDto> Points { get; set; } = new();
    }

    public class MicrorutaPointDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // true si ese tramo queda bloqueado por incidentes
        public bool IsBlocked { get; set; }
    }
}