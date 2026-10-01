using VetCare.Api.Models.Enums;

namespace VetCare.Api.DTOs.Consultas;

public record ConsultaResponse(
    int Id,
    int PetId,
    string NomePet,
    DateTime DataHora,
    string Veterinario,
    string Motivo,
    string? Diagnostico,
    decimal Valor,
    StatusConsulta Status);
