using VetCare.Api.DTOs.Consultas;
using VetCare.Api.Models.Enums;

namespace VetCare.Api.Services.Interfaces;

public interface IConsultaService
{
    Task<IReadOnlyList<ConsultaResponse>> ListarAsync(StatusConsulta? status, CancellationToken ct);
    Task<ConsultaResponse> ObterPorIdAsync(int id, CancellationToken ct);
    Task<ConsultaResponse> CriarAsync(ConsultaRequest request, CancellationToken ct);
    Task<ConsultaResponse> AtualizarAsync(int id, ConsultaRequest request, CancellationToken ct);
    Task RemoverAsync(int id, CancellationToken ct);
}
