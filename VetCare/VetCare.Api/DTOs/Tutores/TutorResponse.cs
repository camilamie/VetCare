namespace VetCare.Api.DTOs.Tutores;

public record TutorResponse(
    int Id,
    string Nome,
    string Cpf,
    string Email,
    string Telefone,
    DateTime DataCadastro,
    int QuantidadePets);
