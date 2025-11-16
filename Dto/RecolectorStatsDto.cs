namespace BackendHorus.DTOs // ajusta al namespace que uses para Dtos
{
    // Para devolver stats al frontend
    public class RecolectorStatsDto
    {
        public int Id { get; set; }
        public int RecolectorId { get; set; }
        public string? RecolectorNombre { get; set; }
        public int BolsasTotales { get; set; }
        public double CoberturaPromedio { get; set; }
        public int CantidadRecorridos { get; set; }
    }

    // Para crear/generar stats a partir de un Recolector
    public class CreateRecolectorStatsDto
    {
        public int RecolectorId { get; set; }
    }
}