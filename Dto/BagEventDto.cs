using System;

namespace BackendHorus.Dto
{
    public class BagEventDto
    {
        public int TripId { get; set; }

        // Por si algún día quieren analizar a qué hora se recogieron las bolsas
        public DateTime? Timestamp { get; set; }
    }
}