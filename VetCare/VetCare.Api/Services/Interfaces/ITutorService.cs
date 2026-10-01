using VetCare.Api.DTOs.Pets;
using VetCare.Api.DTOs.Tutores;

namespace VetCare.Api.Services.Interfaces;

public interface ITutorService
{
    Task<IReadOnlyList<TutorResponse>> ListarAsync(CancellationToken ct);
    Task<TutorResponse> ObterPorIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<PetResponse>> ListarPetsAsync(int tutorId, CancellationToken ct);
    Task<TutorResponse> CriarAsync(TutorRequest request, CancellationToken ct);
    Task<TutorResponse> AtualizarAsync(int id, TutorRequest request, CancellationToken ct);
    Task RemoverAsync(int id, CancellationToken ct);
}
