using System;

namespace BackendHorus.Dto
{
    public class StartTripDto
    {
        public int RecolectorId { get; set; }
        public int MicrorutaId { get; set; }

        // Si viene null, el service usa DateTime.UtcNow
        public DateTime? Inicio { get; set; }
    }
}