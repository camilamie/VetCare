namespace VetCare.Api.DTOs.Pets;

public record PetResponse(
    int Id,
    string Nome,
    string Especie,
    string? Raca,
    DateTime? DataNascimento,
    decimal PesoKg,
    int TutorId,
    string NomeTutor);
