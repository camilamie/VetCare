using System.ComponentModel.DataAnnotations;

namespace VetCare.Api.DTOs.Tutores;

/// <summary>
/// Dados para cadastrar ou atualizar um tutor.
/// </summary>
public class TutorRequest
{
    /// <example>Mariana Souza</example>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; init; } = string.Empty;

    /// <example>12345678901</example>
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos numéricos.")]
    public string Cpf { get; init; } = string.Empty;

    /// <example>mariana.souza@email.com</example>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; init; } = string.Empty;

    /// <example>(11) 98765-4321</example>
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "O telefone deve ter entre 8 e 20 caracteres.")]
    public string Telefone { get; init; } = string.Empty;
}
