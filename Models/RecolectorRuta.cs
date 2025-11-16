using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendHorus.Models
{
    /// <summary>
    /// Relación entre un usuario (recolector), una macroruta y una microruta.
    /// </summary>
    public class RecolectorRuta
    {
        public int Id { get; set; }

        // Id del usuario (por ejemplo, el Id de Identity o similar)
        [Required]
        public string UserId { get; set; } = null!;

        // Claves foráneas
        [ForeignKey(nameof(Macroruta))]
        public int MacrorutaId { get; set; }

        [ForeignKey(nameof(Microruta))]
        public int MicrorutaId { get; set; }

        // Navegación
        public Macroruta Macroruta { get; set; } = null!;
        public Microruta Microruta { get; set; } = null!;
    }
}