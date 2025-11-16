using System.Collections.Generic;

namespace BackendHorus.Models
{
    public class Recolector
    {
        public int Id { get; set; }

        // Nombre de la persona recolectora
        public string Nombre { get; set; } = null!;

        // Por si luego quieren desactivar alguno
        public bool Activo { get; set; } = true;

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}