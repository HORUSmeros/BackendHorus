using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackendHorus.Models;

namespace BackendHorus.Data // ajusta el namespace al que ya usas en tus entities
{
    public class RecolectorStats
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Recolector))]
        public int RecolectorId { get; set; }

        public Recolector Recolector { get; set; } = null!;

        // Siempre >= 100 (lo aseguramos en la lógica)
        [Range(100, int.MaxValue)]
        public int BolsasTotales { get; set; }

        // 0–100 %
        [Range(0, 100)]
        public double CoberturaPromedio { get; set; }

        // Número de recorridos (viajes)
        [Range(0, int.MaxValue)]
        public int CantidadRecorridos { get; set; }
    }
}