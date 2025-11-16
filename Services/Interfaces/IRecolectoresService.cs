using BackendHorus.Dto;
using BackendHorus.Models;

namespace BackendHorus.Services.Interfaces
{
    public interface IRecolectoresService
    {
        Task<IEnumerable<RecolectorStatusDto>> GetStatusAsync(int? macrorutaId);

        Task<IEnumerable<Recolector>> GetAllAsync();
        Task<Recolector?> GetByIdAsync(int id);
        Task<Recolector> CreateAsync(Recolector recolector);
    }
}