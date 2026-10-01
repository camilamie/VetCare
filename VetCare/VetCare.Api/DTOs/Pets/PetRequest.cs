using System.ComponentModel.DataAnnotations;

namespace VetCare.Api.DTOs.Pets;

/// <summary>
/// Dados para cadastrar ou atualizar um pet.
/// </summary>
public class PetRequest
{
    /// <example>Thor</example>
    [Required(ErrorMessage = "O nome do pet é obrigatório.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 80 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    /// <example>Cachorro</example>
    [Required(ErrorMessage = "A espécie é obrigatória.")]
    [StringLength(40, ErrorMessage = "A espécie deve ter no máximo 40 caracteres.")]
    public string Especie { get; init; } = string.Empty;

    /// <example>Golden Retriever</example>
    [StringLength(60, ErrorMessage = "A raça deve ter no máximo 60 caracteres.")]
    public string? Raca { get; init; }

    /// <example>2021-05-10</example>
    public DateTime? DataNascimento { get; init; }

    /// <example>28.5</example>
    [Range(0.01, 999.99, ErrorMessage = "O peso deve estar entre 0,01 e 999,99 kg.")]
    public decimal PesoKg { get; init; }

    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "Informe um TutorId válido.")]
    public int TutorId { get; init; }
}
