using System.ComponentModel.DataAnnotations;
using VetCare.Api.Models.Enums;

namespace VetCare.Api.DTOs.Consultas;

/// <summary>
/// Dados para agendar ou atualizar uma consulta.
/// </summary>
public class ConsultaRequest
{
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "Informe um PetId válido.")]
    public int PetId { get; init; }

    /// <example>2026-12-15T14:30:00</example>
    [Required(ErrorMessage = "A data e hora da consulta são obrigatórias.")]
    public DateTime? DataHora { get; init; }

    /// <example>Dr. Ricardo Lima</example>
    [Required(ErrorMessage = "O nome do veterinário é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do veterinário deve ter entre 3 e 100 caracteres.")]
    public string Veterinario { get; init; } = string.Empty;

    /// <example>Vacinação anual</example>
    [Required(ErrorMessage = "O motivo da consulta é obrigatório.")]
    [StringLength(250, ErrorMessage = "O motivo deve ter no máximo 250 caracteres.")]
    public string Motivo { get; init; } = string.Empty;

    /// <example>Animal saudável</example>
    [StringLength(500, ErrorMessage = "O diagnóstico deve ter no máximo 500 caracteres.")]
    public string? Diagnostico { get; init; }

    /// <example>150.00</example>
    [Range(0, 100000, ErrorMessage = "O valor deve estar entre 0 e 100.000.")]
    public decimal Valor { get; init; }

    /// <example>Agendada</example>
    [EnumDataType(typeof(StatusConsulta), ErrorMessage = "Status inválido. Use: Agendada, Realizada ou Cancelada.")]
    public StatusConsulta Status { get; init; } = StatusConsulta.Agendada;
}
