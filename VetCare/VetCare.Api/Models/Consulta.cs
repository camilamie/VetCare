using VetCare.Api.Models.Enums;

namespace VetCare.Api.Models;

/// <summary>
/// Atendimento veterinário agendado ou realizado para um pet.
/// </summary>
public class Consulta
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Veterinario { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public string? Diagnostico { get; set; }
    public decimal Valor { get; set; }
    public StatusConsulta Status { get; set; } = StatusConsulta.Agendada;

    public int PetId { get; set; }
    public Pet Pet { get; set; } = null!;
}
