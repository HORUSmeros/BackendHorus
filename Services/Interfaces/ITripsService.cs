using BackendHorus.Dto;
using BackendHorus.Models;

namespace BackendHorus.Services.Interfaces
{
    public interface ITripsService
    {
        Task<IEnumerable<TripSummaryDto>> GetTripsAsync(int? macrorutaId, DateTime? date);
        Task<IEnumerable<TripSummaryDto>> GetActiveTripsAsync(int? macrorutaId);

        Task<Trip?> GetTripByIdAsync(int id);
        Task<IEnumerable<PositionSample>?> GetTripPositionsAsync(int tripId);
    }
}