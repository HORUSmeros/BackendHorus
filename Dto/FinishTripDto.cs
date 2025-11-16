using System;

namespace BackendHorus.Dto
{
    public class FinishTripDto
    {
        public int TripId { get; set; }

        // Si viene null, el service usa DateTime.UtcNow
        public DateTime? Fin { get; set; }
    }
}