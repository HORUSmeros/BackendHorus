using BackendHorus.Dto;

namespace BackendHorus.Services.Interfaces
{
    public interface IRutasService
    {
        Task<IEnumerable<MacrorutaDto>> GetMacrorutasAsync();
        Task<IEnumerable<MicrorutaDto>> GetMicrorutasAsync(int? macrorutaId);
        Task<MicrorutaDto?> GetMicrorutaByIdAsync(int id);
    }
}