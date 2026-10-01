using System.Linq.Expressions;
using VetCare.Api.DTOs.Consultas;
using VetCare.Api.Models;

namespace VetCare.Api.Mappings;

public static class ConsultaMappings
{
    public static readonly Expression<Func<Consulta, ConsultaResponse>> ParaResponse = c =>
        new ConsultaResponse(c.Id, c.PetId, c.Pet.Nome, c.DataHora, c.Veterinario,
            c.Motivo, c.Diagnostico, c.Valor, c.Status);

    public static void AplicarDados(this Consulta consulta, ConsultaRequest request)
    {
        consulta.PetId = request.PetId;
        consulta.DataHora = request.DataHora!.Value;
        consulta.Veterinario = request.Veterinario.Trim();
        consulta.Motivo = request.Motivo.Trim();
        consulta.Diagnostico = string.IsNullOrWhiteSpace(request.Diagnostico) ? null : request.Diagnostico.Trim();
        consulta.Valor = request.Valor;
        consulta.Status = request.Status;
    }
}
