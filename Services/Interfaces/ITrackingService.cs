using BackendHorus.Dto;
using BackendHorus.Models;

namespace BackendHorus.Services.Interfaces
{
    public interface ITrackingService
    {
        Task<Trip> StartTripAsync(StartTripDto dto);
        Task<Trip?> FinishTripAsync(FinishTripDto dto);

        Task<bool> RegisterPositionAsync(PositionUpdateDto dto);
        Task<bool> RegisterBagAsync(BagEventDto dto);
        Task<bool> RegisterIncidentAsync(IncidentDto dto);
    }
}