namespace BackendHorus.Dto
{
    public class RecolectorRutaDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public int MacrorutaId { get; set; }
        public string? MacrorutaNombre { get; set; }

        public int MicrorutaId { get; set; }
        public string? MicrorutaNombre { get; set; }
    }

    public class CreateRecolectorRutaDto
    {
        public string UserId { get; set; } = null!;
        public int MacrorutaId { get; set; }
        public int MicrorutaId { get; set; }
    }
}