using VetCare.Api.DTOs.Consultas;
using VetCare.Api.DTOs.Pets;

namespace VetCare.Api.Services.Interfaces;

public interface IPetService
{
    Task<IReadOnlyList<PetResponse>> ListarAsync(string? especie, CancellationToken ct);
    Task<PetResponse> ObterPorIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<ConsultaResponse>> ListarConsultasAsync(int petId, CancellationToken ct);
    Task<PetResponse> CriarAsync(PetRequest request, CancellationToken ct);
    Task<PetResponse> AtualizarAsync(int id, PetRequest request, CancellationToken ct);
    Task RemoverAsync(int id, CancellationToken ct);
}
