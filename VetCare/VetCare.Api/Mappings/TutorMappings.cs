using System.Linq.Expressions;
using VetCare.Api.DTOs.Tutores;
using VetCare.Api.Models;

namespace VetCare.Api.Mappings;

public static class TutorMappings
{
    /// <summary>Projeção traduzida em SQL pelo EF Core (evita carregar a entidade inteira).</summary>
    public static readonly Expression<Func<Tutor, TutorResponse>> ParaResponse = t =>
        new TutorResponse(t.Id, t.Nome, t.Cpf, t.Email, t.Telefone, t.DataCadastro, t.Pets.Count);

    public static void AplicarDados(this Tutor tutor, TutorRequest request)
    {
        tutor.Nome = request.Nome.Trim();
        tutor.Cpf = request.Cpf;
        tutor.Email = request.Email.Trim().ToLowerInvariant();
        tutor.Telefone = request.Telefone.Trim();
    }
}
