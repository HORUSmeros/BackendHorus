using System;

namespace BackendHorus.Dto
{
    public class PositionUpdateDto
    {
        public int TripId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Opcional; si null, el service pone la hora del servidor
        public DateTime? Timestamp { get; set; }
    }
}